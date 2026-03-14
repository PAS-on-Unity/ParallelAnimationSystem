using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ParallelAnimationSystem.Core.Data;
using ParallelAnimationSystem.Data;
using ParallelAnimationSystem.Rendering;
using ParallelAnimationSystem.Rendering.Data;
using UnityEngine;

namespace ParallelAnimationSystem.Unity.Implementation;

public class Renderer : IRenderer, IDisposable
{
    private static readonly int AllVerticesId = Shader.PropertyToID("_AllVertices");
    private static readonly int AllIndicesId = Shader.PropertyToID("_AllIndices");
    private static readonly int MeshInfosId = Shader.PropertyToID("_MeshInfos");
    private static readonly int MeshDrawItemsId = Shader.PropertyToID("_MeshDrawItems");
    
    [StructLayout(LayoutKind.Sequential)]
    private struct MeshInfo
    {
        public int vertexOffset;
        public int indexOffset;
        public int indexCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct GPUMeshDrawItem
    {
        public float m11, m12, m21, m22, m31, m32;
        public float pad0, pad1;
        public Vector4 color1;
        public Vector4 color2;
        public int renderMode;
        public int meshID;
        public float gradientRotation;
        public float gradientScale;
    }
    
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    
    private CameraState cameraState;
    private PostProcessingState postProcessingState;
    private ColorRgba clearColor;
    
    private DrawCommand[] drawCommands = new DrawCommand[1000];
    private int drawCommandsCount = 0;
    
    private MeshDrawItem[] meshDrawItems = new MeshDrawItem[1000];
    private TextDrawItem[] textDrawItems = new TextDrawItem[1000];
    
    private MeshInfo[] meshInfos = new MeshInfo[1000];
    
    private GraphicsBuffer allVerticesBuffer = new(GraphicsBuffer.Target.Structured, 1000, UnsafeUtil.SizeOf<Vector2>());
    private GraphicsBuffer allIndicesBuffer = new(GraphicsBuffer.Target.Structured, 1000, sizeof(int));
    private GraphicsBuffer meshInfosBuffer = new(GraphicsBuffer.Target.Structured, 1000, UnsafeUtil.SizeOf<MeshInfo>());
    private GraphicsBuffer meshDrawItemsBuffer = new(GraphicsBuffer.Target.Structured, 1000, UnsafeUtil.SizeOf<GPUMeshDrawItem>());
    private GraphicsBuffer indirectDrawBuffer = new(GraphicsBuffer.Target.IndirectArguments, 1000, Unsafe.SizeOf<GraphicsBuffer.IndirectDrawArgs>());
    
    private readonly List<GPUMeshDrawItem> gpuMeshDrawItems = [];
    private readonly List<GraphicsBuffer.IndirectDrawArgs> gpuIndirectDrawArgsItems = [];
    
    private readonly PASUnityAssets assets;
    private readonly RenderingFactory renderingFactory;
    
    private bool meshesDirty = true;

    public Renderer(PASUnityAssets assets, IRenderingFactory renderingFactory)
    {
        this.assets = assets;
        this.renderingFactory = (RenderingFactory)renderingFactory;
        
        this.renderingFactory.Meshes.ItemInserted += OnMeshInserted;
        this.renderingFactory.Meshes.ItemRemoved += OnMeshRemoved;
    }

    public void Dispose()
    {
        renderingFactory.Meshes.ItemInserted -= OnMeshInserted;
        renderingFactory.Meshes.ItemRemoved -= OnMeshRemoved;
        
        allVerticesBuffer.Dispose();
        allIndicesBuffer.Dispose();
        meshInfosBuffer.Dispose();
        meshDrawItemsBuffer.Dispose();
        indirectDrawBuffer.Dispose();
    }

    private void OnMeshInserted(object sender, ObservableSparseSetEventArgs<MeshData> e)
    {
        meshesDirty = true;
    }

    private void OnMeshRemoved(object sender, ObservableSparseSetEventArgs<MeshData> e)
    {
        meshesDirty = true;
    }

    public void ProcessFrame(IDrawDataProvider drawDataProvider)
    {
        var drawData = drawDataProvider.CreateDrawData();

        cameraState = drawData.CameraState;
        postProcessingState = drawData.PostProcessingState;
        clearColor = drawData.ClearColor;

        if (drawData.DrawCommands.Length > drawCommands.Length)
            Array.Resize(ref drawCommands, drawData.DrawCommands.Length);
        drawData.DrawCommands.CopyTo(drawCommands);
        drawCommandsCount = drawData.DrawCommands.Length;

        if (drawData.MeshDrawItems.Length > meshDrawItems.Length)
            Array.Resize(ref meshDrawItems, drawData.MeshDrawItems.Length);
        drawData.MeshDrawItems.CopyTo(meshDrawItems);

        if (drawData.TextDrawItems.Length > textDrawItems.Length)
            Array.Resize(ref textDrawItems, drawData.TextDrawItems.Length);
        drawData.TextDrawItems.CopyTo(textDrawItems);
    }

    public void Update(Camera camera)
    {
        camera.backgroundColor = new Color(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
        camera.transform.localPosition = new Vector3(cameraState.Position.X, cameraState.Position.Y, 0f);
        camera.transform.localRotation = Quaternion.Euler(0f, 0f, cameraState.Rotation * Mathf.Rad2Deg);
        camera.orthographicSize = cameraState.Scale;

        UpdateResources();
    }

    public void Render()
    {
        gpuMeshDrawItems.Clear();
        gpuIndirectDrawArgsItems.Clear();

        for (var i = 0; i < drawCommandsCount; i++)
        {
            ref var cmd = ref drawCommands[i];
            if (cmd.DrawType != DrawType.Mesh)
                continue; // TODO: support other draw types
            
            ref var meshDrawItem = ref meshDrawItems[cmd.DrawId];
            gpuMeshDrawItems.Add(new GPUMeshDrawItem
            {
                m11 = meshDrawItem.Transform.M11,
                m12 = meshDrawItem.Transform.M12,
                m21 = meshDrawItem.Transform.M21,
                m22 = meshDrawItem.Transform.M22,
                m31 = meshDrawItem.Transform.M31,
                m32 = meshDrawItem.Transform.M32,
                color1 = new Vector4(meshDrawItem.Color1.R, meshDrawItem.Color1.G, meshDrawItem.Color1.B, meshDrawItem.Color1.A),
                color2 = new Vector4(meshDrawItem.Color2.R, meshDrawItem.Color2.G, meshDrawItem.Color2.B, meshDrawItem.Color2.A),
                renderMode = (int)meshDrawItem.RenderMode,
                meshID = meshDrawItem.MeshHandle.Id,
                gradientRotation = meshDrawItem.GradientRotation,
                gradientScale = meshDrawItem.GradientScale
            });
            
            gpuIndirectDrawArgsItems.Add(new GraphicsBuffer.IndirectDrawArgs
            {
                vertexCountPerInstance = unchecked((uint)meshInfos[meshDrawItem.MeshHandle.Id].indexCount),
                instanceCount = 1,
                startVertex = 0,
                startInstance = 0
            });
        }
        
        // upload draw items to GPU
        if (gpuMeshDrawItems.Count > meshDrawItemsBuffer.count)
        {
            meshDrawItemsBuffer.Dispose();
            meshDrawItemsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, gpuMeshDrawItems.Count, UnsafeUtil.SizeOf<GPUMeshDrawItem>());
        }
        meshDrawItemsBuffer.SetData(gpuMeshDrawItems);
        
        if (gpuIndirectDrawArgsItems.Count > indirectDrawBuffer.count)
        {
            indirectDrawBuffer.Dispose();
            indirectDrawBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, gpuIndirectDrawArgsItems.Count, Unsafe.SizeOf<GraphicsBuffer.IndirectDrawArgs>());
        }
        indirectDrawBuffer.SetData(gpuIndirectDrawArgsItems);
        
        var material = assets.material;
        material.SetBuffer(AllVerticesId, allVerticesBuffer);
        material.SetBuffer(AllIndicesId, allIndicesBuffer);
        material.SetBuffer(MeshInfosId, meshInfosBuffer);
        material.SetBuffer(MeshDrawItemsId, meshDrawItemsBuffer);
        
        var rp = new RenderParams(material)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 10000f)
        };

        Graphics.RenderPrimitivesIndirect(rp, MeshTopology.Triangles, indirectDrawBuffer, gpuIndirectDrawArgsItems.Count);
    }

    private void UpdateResources()
    {
        UpdateMeshes();
    }

    private void UpdateMeshes()
    {
        if (!meshesDirty)
            return;
        meshesDirty = false;
        
        // rebuild mesh buffer
        var maxMeshId = -1;
        foreach (var (id, _) in renderingFactory.Meshes)
            maxMeshId = Math.Max(maxMeshId, id);
        
        if (meshInfos.Length <= maxMeshId)
            Array.Resize(ref meshInfos, maxMeshId + 1);
        
        var allVertices = new List<System.Numerics.Vector2>();
        var allIndices = new List<int>();
        
        foreach (var (id, meshData) in renderingFactory.Meshes)
        {
            var vertexOffset = allVertices.Count;
            var indexOffset = allIndices.Count;
            
            allVertices.AddRange(meshData.Vertices);
            allIndices.AddRange(meshData.Indices);
            
            meshInfos[id] = new MeshInfo
            {
                vertexOffset = vertexOffset,
                indexOffset = indexOffset,
                indexCount = meshData.Indices.Length
            };
        }
        
        // upload to GPU
        if (allVertices.Count > allVerticesBuffer.count)
        {
            allVerticesBuffer.Dispose();
            allVerticesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured , allVertices.Count, UnsafeUtil.SizeOf<Vector2>());
        }
        allVerticesBuffer.SetData(allVertices);
        
        if (allIndices.Count > allIndicesBuffer.count)
        {
            allIndicesBuffer.Dispose();
            allIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, allIndices.Count, sizeof(int));
        }
        allIndicesBuffer.SetData(allIndices);
        
        if (meshInfos.Length > meshInfosBuffer.count)
        {
            meshInfosBuffer.Dispose();
            meshInfosBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, meshInfos.Length, UnsafeUtil.SizeOf<MeshInfo>());
        }
        meshInfosBuffer.SetData(meshInfos);
    }
}
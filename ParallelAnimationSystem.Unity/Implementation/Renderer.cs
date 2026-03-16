using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ParallelAnimationSystem.Core.Data;
using ParallelAnimationSystem.Data;
using ParallelAnimationSystem.Rendering;
using ParallelAnimationSystem.Rendering.Data;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

namespace ParallelAnimationSystem.Unity.Implementation;

public class Renderer : IRenderer, IDisposable
{
    private static readonly int AllVerticesId = Shader.PropertyToID("_AllVertices");
    private static readonly int DrawItemsId = Shader.PropertyToID("_DrawItems");
    
    private struct MeshInfo
    {
        public int indexOffset;
        public int indexCount;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct GPUGlyphItem
    {
        Vector2 min;
        Vector2 max;
        Vector2 minUV;
        Vector2 maxUV;
        Vector4 color;
        float rotation;
        int boldItalic;
        int atlasIndex;
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct GPUDrawItem
    {
        public Matrix3x2 transform;
        public Vector4 color1;
        public Vector4 color2;
        public int renderMode;
        public float gradientRotation;
        public float gradientScale;
    }
    
    private CameraState cameraState;
    private PostProcessingState postProcessingState;
    private ColorRgba clearColor;
    
    private DrawCommand[] drawCommands = new DrawCommand[1000];
    private int drawCommandsCount = 0;
    
    private MeshDrawItem[] meshDrawItems = new MeshDrawItem[1000];
    private TextDrawItem[] textDrawItems = new TextDrawItem[1000];
    
    private MeshInfo[] meshInfos = new MeshInfo[1000];
    
    private GraphicsBuffer allVerticesBuffer = new(GraphicsBuffer.Target.Structured, 1000, Unsafe.SizeOf<Vector2>());
    private GraphicsBuffer allIndicesBuffer = new(GraphicsBuffer.Target.Structured, 1000, sizeof(int));
    private GraphicsBuffer meshDrawItemsBuffer = new(GraphicsBuffer.Target.Structured, 1000, Unsafe.SizeOf<GPUDrawItem>());
    private GraphicsBuffer indirectDrawBuffer = new(GraphicsBuffer.Target.IndirectArguments, 1000, Unsafe.SizeOf<GraphicsBuffer.IndirectDrawIndexedArgs>());
    
    private readonly List<GPUDrawItem> gpuMeshDrawItems = [];
    private readonly List<GraphicsBuffer.IndirectDrawIndexedArgs> gpuIndirectDrawIndexedArgsItems = [];
    
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
        gpuIndirectDrawIndexedArgsItems.Clear();

        for (var i = 0; i < drawCommandsCount; i++)
        {
            ref var cmd = ref drawCommands[i];
            if (cmd.DrawType != DrawType.Mesh)
                continue; // TODO: support other draw types
            
            ref var meshDrawItem = ref meshDrawItems[cmd.DrawId];
            ref var meshInfo = ref meshInfos[meshDrawItem.MeshHandle.Id];
            
            gpuMeshDrawItems.Add(new GPUDrawItem
            {
                transform = meshDrawItem.Transform,
                color1 = new Vector4(meshDrawItem.Color1.R, meshDrawItem.Color1.G, meshDrawItem.Color1.B, meshDrawItem.Color1.A),
                color2 = new Vector4(meshDrawItem.Color2.R, meshDrawItem.Color2.G, meshDrawItem.Color2.B, meshDrawItem.Color2.A),
                renderMode = (int)meshDrawItem.RenderMode,
                gradientRotation = meshDrawItem.GradientRotation,
                gradientScale = meshDrawItem.GradientScale
            });
            
            gpuIndirectDrawIndexedArgsItems.Add(new GraphicsBuffer.IndirectDrawIndexedArgs
            {
                indexCountPerInstance = unchecked((uint)meshInfo.indexCount),
                instanceCount = 1,
                startIndex = unchecked((uint)meshInfo.indexOffset),
                baseVertexIndex = 0,
                startInstance = 0
            });
        }
        
        // upload draw items to GPU
        if (gpuMeshDrawItems.Count > meshDrawItemsBuffer.count)
        {
            meshDrawItemsBuffer.Dispose();
            meshDrawItemsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, gpuMeshDrawItems.Count, Unsafe.SizeOf<GPUDrawItem>());
        }
        meshDrawItemsBuffer.SetData(gpuMeshDrawItems);
        
        if (gpuIndirectDrawIndexedArgsItems.Count > indirectDrawBuffer.count)
        {
            indirectDrawBuffer.Dispose();
            indirectDrawBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, gpuIndirectDrawIndexedArgsItems.Count, Unsafe.SizeOf<GraphicsBuffer.IndirectDrawIndexedArgs>());
        }
        indirectDrawBuffer.SetData(gpuIndirectDrawIndexedArgsItems);
        
        var material = assets.material;
        material.SetBuffer(AllVerticesId, allVerticesBuffer);
        material.SetBuffer(DrawItemsId, meshDrawItemsBuffer);
        
        var rp = new RenderParams(material)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 10000f)
        };

        Graphics.RenderPrimitivesIndexedIndirect(rp, MeshTopology.Triangles, allIndicesBuffer, indirectDrawBuffer, gpuIndirectDrawIndexedArgsItems.Count);
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
            
            var indicesCopy = new int[meshData.Indices.Length];
            for (var i = 0; i < meshData.Indices.Length; i++) // add vertex offset to indices
                indicesCopy[i] = meshData.Indices[i] + vertexOffset;
            
            allVertices.AddRange(meshData.Vertices);
            allIndices.AddRange(indicesCopy);
            
            meshInfos[id] = new MeshInfo
            {
                indexOffset = indexOffset,
                indexCount = meshData.Indices.Length
            };
        }
        
        // upload to GPU
        if (allVertices.Count > allVerticesBuffer.count)
        {
            allVerticesBuffer.Dispose();
            allVerticesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured , allVertices.Count, Unsafe.SizeOf<Vector2>());
        }
        allVerticesBuffer.SetData(allVertices);
        
        if (allIndices.Count > allIndicesBuffer.count)
        {
            allIndicesBuffer.Dispose();
            allIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, allIndices.Count, sizeof(int));
        }
        allIndicesBuffer.SetData(allIndices);
    }
}
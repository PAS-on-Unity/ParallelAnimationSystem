using System;
using ParallelAnimationSystem.Core.Text;
using ParallelAnimationSystem.Data;
using ParallelAnimationSystem.Rendering;
using ParallelAnimationSystem.Rendering.Handle;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ParallelAnimationSystem.Unity.Implementation;

public class RenderingFactory : IRenderingFactory
{
    public ObservableSparseSet<MeshData> Meshes { get; } = new();
    
    private int dummyFontId = 0;
    private int dummyTextId = 0;
    
    public MeshHandle CreateMesh(ReadOnlySpan<Vector2> vertices, ReadOnlySpan<int> indices)
    {
        var id = Meshes.Insert(new MeshData
        {
            Vertices = vertices.ToArray(),
            Indices = indices.ToArray()
        });
        return new MeshHandle(id);
    }

    public void DestroyMesh(MeshHandle handle)
    {
        Meshes.Remove(handle.Id, out _);
    }

    public FontHandle CreateFont(int width, int height, ReadOnlySpan<byte> atlas)
    {
        return new FontHandle(dummyFontId++); // TODO: implement font creation
    }

    public void DestroyFont(FontHandle handle)
    {
        // TODO: implement font destruction
    }

    public TextHandle CreateText(ShapedRichText richText)
    {
        return new TextHandle(dummyTextId++);  // TODO: implement text creation
    }

    public void DestroyText(TextHandle handle)
    {
        // TODO: implement text destruction
    }
}
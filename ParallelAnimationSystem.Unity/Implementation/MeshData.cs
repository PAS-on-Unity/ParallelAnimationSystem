using System.Numerics;

namespace ParallelAnimationSystem.Unity.Implementation;

public class MeshData
{
    public required Vector2[] Vertices { get; init; }
    public required int[] Indices { get; init; }
}
using ParallelAnimationSystem.Core;
using ParallelAnimationSystem.Rendering;

namespace ParallelAnimationSystem;

public class App : IDisposable
{
    public IRenderer Renderer { get; }
    public BeatmapRunner BeatmapRunner { get; }

    internal App(IRenderer renderer, BeatmapRunner beatmapRunner)
    {
        Renderer = renderer;
        BeatmapRunner = beatmapRunner;
    }
    
    public void Dispose()
    {
        Renderer.Dispose();
    }
}
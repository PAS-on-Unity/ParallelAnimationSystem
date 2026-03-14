using ParallelAnimationSystem.Mathematics;
using ParallelAnimationSystem.Windowing;

namespace ParallelAnimationSystem.Unity.Implementation;

public class Window : IWindow
{
    public Vector2i FramebufferSize { get; } = Vector2i.Zero;
    
    public bool ShouldClose => false;
    
    public void PollEvents()
    {
    }

    public void Close()
    {
    }
}
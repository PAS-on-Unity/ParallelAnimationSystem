using ParallelAnimationSystem.Data;
using ParallelAnimationSystem.Rendering;

namespace ParallelAnimationSystem;

public interface IStartup
{
    IAppSettings CreateAppSettings();
    ILogger CreateLogger();
    IResourceManager? CreateResourceManager();
    IRenderer CreateRenderer();
    IMediaProvider CreateMediaProvider();
}
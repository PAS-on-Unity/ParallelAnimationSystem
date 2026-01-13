using ParallelAnimationSystem.Core;
using ParallelAnimationSystem.Data;

namespace ParallelAnimationSystem;

public static class StartupExtension
{
    public static App InitializeApp(this IStartup startup)
    {
        // Create renderer
        var renderer = startup.CreateRenderer();
        
        // Create beatmap runner
        var appSettings = startup.CreateAppSettings();
        var mediaProvider = startup.CreateMediaProvider();
        
        var ownResourceManager = new EmbeddedResourceManager(typeof(StartupExtension).Assembly);
        var appResourceManager = startup.CreateResourceManager();
        IResourceManager resourceManager = appResourceManager is null
            ? ownResourceManager
            : new MergedResourceManager([appResourceManager, ownResourceManager]);
        var logger = startup.CreateLogger();
        var beatmapRunner = new BeatmapRunner(appSettings, mediaProvider, resourceManager, renderer, logger);
        
        // Initialize them
        beatmapRunner.Initialize();
        renderer.Initialize();
        
        return new App(renderer, beatmapRunner);
    }
}
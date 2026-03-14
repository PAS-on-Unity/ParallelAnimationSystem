using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ParallelAnimationSystem.Core;
using ParallelAnimationSystem.Rendering;
using ParallelAnimationSystem.Unity.Implementation;
using UnityEngine;
using Renderer = ParallelAnimationSystem.Unity.Implementation.Renderer;
using RenderQueue = ParallelAnimationSystem.Rendering.RenderQueue;

namespace ParallelAnimationSystem.Unity;

public class PASComponent : MonoBehaviour
{
    public float time;
    public required Camera renderCamera;
    public required PASUnityAssets assets;
    
    public IServiceProvider ServiceProvider => serviceProvider;
    public IServiceScope AppScope => appScope;
    public IServiceScope RenderScope => renderScope;
    
    private ServiceProvider serviceProvider = null!;
    
    private RenderQueue renderQueue = null!;
    
    private IServiceScope appScope = null!;
    private AppDirector director = null!;
    
    private IServiceScope renderScope = null!;
    private new Renderer renderer = null!;
    
    private void Awake()
    {
        var services = new ServiceCollection();

        services.AddSingleton(assets);

        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddProvider(new UnityLoggerProvider());
        });
        
        services.AddPAS(builder =>
        {
            var appSettings = new AppSettings
            {
                AspectRatio = null,
                EnablePostProcessing = true,
                EnableTextRendering = true
            };
            
            builder.UseAppSettings(appSettings);
            builder.UseRenderer<Renderer>();
            builder.UseRenderingFactory<RenderingFactory>();
            builder.UseRenderQueue<RenderQueue>();
            builder.UseWindow<Window>();
        });
        
        serviceProvider = services.BuildServiceProvider();
        renderQueue = (RenderQueue)serviceProvider.GetRequiredService<IRenderQueue>();
        
        appScope = serviceProvider.CreateScope();
        director = appScope.ServiceProvider.GetRequiredService<AppDirector>();
        
        renderScope = serviceProvider.CreateScope();
        renderer = (Renderer)renderScope.ServiceProvider.GetRequiredService<IRenderer>();
    }

    private void Update()
    {
        director.ProcessFrame(time);
        renderQueue.ProcessFrame(renderer);
        
        renderer.Update(renderCamera);
        renderer.Render();
    }

    private void OnDestroy()
    {
        serviceProvider.Dispose();
    }
}
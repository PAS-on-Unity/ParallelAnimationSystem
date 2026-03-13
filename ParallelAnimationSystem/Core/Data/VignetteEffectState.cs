using System.Numerics;
using ParallelAnimationSystem.Mathematics;

namespace ParallelAnimationSystem.Core.Data;

public struct VignetteEffectState()
{
    public float Intensity { get; set; } = 0.0f;
    public float Smoothness { get; set; } = 0.0f;
    public ColorRgb Color { get; set; } = default;
    public bool Rounded { get; set; } = false;
    public float Roundness { get; set; } = 0.0f;
    public Vector2 Center { get; set; } = Vector2.Zero;
    
    public static VignetteEffectState Lerp(VignetteEffectState a, VignetteEffectState b, float t)
        => new()
        {
            Intensity = MathUtil.Lerp(a.Intensity, b.Intensity, t),
            Smoothness = MathUtil.Lerp(a.Smoothness, b.Smoothness, t),
            Color = ColorRgb.Lerp(a.Color, b.Color, t),
            Rounded = a.Rounded, // zero order hold, can't lerp booleans
            Roundness = MathUtil.Lerp(a.Roundness, b.Roundness, t),
            Center = Vector2.Lerp(a.Center, b.Center, t)
        };
}
using System.Numerics;
using ParallelAnimationSystem.Core.Data;
using ParallelAnimationSystem.Data;

namespace ParallelAnimationSystem.Rendering;

public interface IDrawList
{
    CameraData CameraData { get; set; }
    PostProcessingData PostProcessingData { get; set; }
    ColorRgba ClearColor { get; set; }

    void AddMesh(IMeshHandle mesh, Matrix3x2 transform, ColorRgba color1, ColorRgba color2, RenderMode renderMode,
        float gradientRotation, float gradientScale);

    void AddText(ITextHandle text, Matrix3x2 transform, ColorRgba color);
}
namespace TmpIO;

public record struct TmpGlyph(
    int Id,
    float Advance,
    float BearingX,
    float BearingY,
    float Width,
    float Height,
    float MinX,
    float MinY,
    float MaxX,
    float MaxY);
using MsdfgenNet;

namespace TmpGenerator;

public record Glyph(int Id, float Advance, float BearingX, float BearingY, float Width, float Height, Bitmap? Bitmap);
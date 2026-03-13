namespace TmpParser;

public readonly record struct ColorAlpha(Color3? Rgb, int? A)
{
    public override string ToString()
    {
        return $"ColorAlpha(Rgb: {Rgb?.ToString() ?? "Inherit"}, A: {A?.ToString() ?? "Inherit"})";
    }
}
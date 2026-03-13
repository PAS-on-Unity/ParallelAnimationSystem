namespace TmpParser;

public class StyleElement : IElement
{
    public bool? Bold { get; set; }
    public bool? Italic { get; set; }
    public bool? Underline { get; set; }
    public ColorAlpha? Color { get; set; }

    public override string ToString()
        => $"StyleElement({
            string.Join(", ", Enumerable.Empty<string?>()
                .Append(Bold.HasValue ? $"Bold: {Bold.Value}" : null)
                .Append(Italic.HasValue ? $"Italic: {Italic.Value}" : null)
                .Append(Underline.HasValue ? $"Underline: {Underline.Value}" : null)
                .Append(Color.HasValue ? $"Color: {Color.Value}" : null)
                .OfType<string>())})";
}
namespace TmpParser;

public class TextElement : IElement
{
    public required string Value { get; set; }

    public override string ToString()
        => $"TextElement({Value})";
}
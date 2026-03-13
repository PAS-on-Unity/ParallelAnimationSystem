namespace TmpParser;

public class FontElement : IElement
{
    public required string? Value { get; set; }
    
    public override string ToString()
        => $"FontElement({Value ?? "Inherit"})";
}
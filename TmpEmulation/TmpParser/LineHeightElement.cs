namespace TmpParser;

public class LineHeightElement : IElement
{
    public Measurement? Value { get; set; }
    
    public override string ToString()
        => $"LineHeightElement({Value})";
}
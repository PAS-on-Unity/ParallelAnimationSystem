namespace TmpParser;

public class SizeElement : IElement
{
    public Measurement Value { get; set; }
    
    public override string ToString()
        => $"SizeElement({Value})";
}
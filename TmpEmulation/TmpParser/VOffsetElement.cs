namespace TmpParser;

public class VOffsetElement : IElement
{
    public Measurement Value { get; set; }
    
    public override string ToString()
        => $"VOffsetElement({Value})";
}
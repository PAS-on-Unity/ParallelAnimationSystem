namespace TmpParser;

public class PosElement : IElement
{
    public Measurement Value { get; set; }
    
    public override string ToString()
        => $"PosElement({Value})";
}
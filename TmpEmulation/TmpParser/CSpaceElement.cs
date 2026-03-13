namespace TmpParser;

public class CSpaceElement : IElement
{
    public Measurement Value { get; set; }
    
    public override string ToString()
        => $"CSpaceElement({Value})";
}
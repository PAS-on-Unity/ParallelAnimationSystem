namespace TmpParser;

public class MarkElement : IElement
{
    public ColorAlpha Value { get; set; }
    
    public override string ToString()
        => $"MarkElement({Value})";
}

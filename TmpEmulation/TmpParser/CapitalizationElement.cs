namespace TmpParser;

public class CapitalizationElement : IElement
{
    public Capitalization Value { get; set; }
    
    public override string ToString()
        => $"CapitalizationElement({Value})";
}
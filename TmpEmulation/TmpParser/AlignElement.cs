namespace TmpParser;

public class AlignElement : IElement
{
    public HorizontalAlignment? Alignment { get; set; }
    
    public override string ToString()
        => $"AlignElement({Alignment?.ToString() ?? "Inherit"})";
}
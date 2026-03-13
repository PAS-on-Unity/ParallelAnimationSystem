namespace TmpParser;

public class RotateElement : IElement
{
    public required float Angle { get; init; }
    
    public override string ToString()
        => $"RotateElement({Angle})";
}
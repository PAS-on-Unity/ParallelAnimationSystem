namespace TmpParser;

public class TagParsingState
{
    public int BoldNestLevel { get; set; }
    public int ItalicNestLevel { get; set; }
    public int UnderlineNestLevel { get; set; }
    public Measurement VOffset { get; set; }
    public Capitalization Capitalization { get; set; }
    public Stack<Measurement> CSpaceStack { get; } = [];
    public Stack<Measurement> LineHeightStack { get; } = [];
    public Stack<ColorAlpha> ColorStack { get; } = [];
    public Stack<ColorAlpha> MarkStack { get; } = [];
    public Stack<float> RotateStack { get; } = [];
    public Stack<string> FontStack { get; } = [];
}
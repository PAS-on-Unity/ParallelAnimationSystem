namespace TmpParser;

public class TextToken(string value) : IToken
{
    public TokenType Type => TokenType.Text;
    public string Value { get; set; } = value;
    
    public override string ToString()
        => $"TextToken({Value})";
}
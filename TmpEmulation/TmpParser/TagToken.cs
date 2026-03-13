namespace TmpParser;

public class TagToken(string value, string originalValue) : IToken
{
    public TokenType Type => TokenType.Tag;
    public string Value { get; set; } = value;
    public string OriginalValue { get; set; } = originalValue;

    public override string ToString()
        => $"TagToken({Value}, {OriginalValue})";
}
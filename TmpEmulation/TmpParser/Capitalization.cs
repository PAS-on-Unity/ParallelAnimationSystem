namespace TmpParser;

[Flags]
public enum Capitalization
{
    None = 0,
    Uppercase = 0b001,
    Lowercase = 0b010,
    SmallCaps = 0b100,
}
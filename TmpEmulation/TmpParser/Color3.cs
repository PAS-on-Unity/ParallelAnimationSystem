using System.Globalization;

namespace TmpParser;

public readonly record struct Color3(byte R, byte G, byte B)
{
    public override string ToString()
    {
        return $"#{R:X2}{G:X2}{B:X2}";
    }

    public static Color3 ParseHex(string hex)
    {
        return new Color3(
            byte.Parse(hex[..2], NumberStyles.HexNumber),
            byte.Parse(hex[2..4], NumberStyles.HexNumber),
            byte.Parse(hex[4..6], NumberStyles.HexNumber)
        );
    }
    
    public static bool TryParseHex(string hex, out Color3 color)
    {
        color = default;
        
        if (string.IsNullOrWhiteSpace(hex))
            return false;
        
        if (hex.Length != 6)
            return false;
        
        if (byte.TryParse(hex[..2], NumberStyles.HexNumber, null, out var r) &&
            byte.TryParse(hex[2..4], NumberStyles.HexNumber, null, out var g) &&
            byte.TryParse(hex[4..6], NumberStyles.HexNumber, null, out var b))
        {
            color = new Color3(r, g, b);
            return true;
        }
        
        return false;
    }
}
namespace TmpParser;

public record struct Measurement(float Value, Unit Unit)
{
    public override string ToString()
        => $"Measurement({Value}, {Unit})";
    
    public static bool TryParse(string s, out Measurement result)
    {
        s = s.Trim();
        
        if (float.TryParse(s, out var value))
        {
            result = new Measurement(value, Unit.Pixel);
            return true;
        }
        
        if (s.EndsWith("px") && float.TryParse(s[..^2].Trim(), out var pxValue))
        {
            result = new Measurement(pxValue, Unit.Pixel);
            return true;
        }
        
        if (s.EndsWith('%') && float.TryParse(s[..^1].Trim(), out var percentValue))
        {
            result = new Measurement(percentValue / 100.0f, Unit.Percent);
            return true;
        }
        
        if (s.EndsWith("em"))
        {
            var sEmValue = s[..^2].Trim();

            if (string.IsNullOrWhiteSpace(sEmValue))
            {
                result = new Measurement(0.0f, Unit.Em);
                return true;
            }
            
            if (float.TryParse(sEmValue, out var emValue))
            {
                result = new Measurement(emValue, Unit.Em);
                return true;
            }
        }
        
        result = default;
        return false;
    }
}
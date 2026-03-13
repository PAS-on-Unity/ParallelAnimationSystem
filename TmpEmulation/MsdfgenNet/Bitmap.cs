namespace MsdfgenNet;

public class Bitmap(int width, int height, int channels)
{
    public int Channels { get; } = channels;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public Span<float> Data => data;
    
    private float[] data = new float[width * height * channels];
}
using System.Runtime.InteropServices;

namespace TmpIO;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TmpAtlasPixel
{
    // Half-precision floating-point format (16 bits per channel)
    public ushort R;
    public ushort G;
    public ushort B;
}
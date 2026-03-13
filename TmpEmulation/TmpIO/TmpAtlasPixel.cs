using System.Runtime.InteropServices;

namespace TmpIO;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TmpAtlasPixel
{
    public Half R;
    public Half G;
    public Half B;
}
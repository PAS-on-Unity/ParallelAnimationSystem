using System.Runtime.InteropServices;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct BitmapRef(IntPtr Data, int Width, int Height);

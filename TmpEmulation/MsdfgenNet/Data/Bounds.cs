using System.Runtime.InteropServices;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct Bounds(double Left, double Bottom, double Right, double Top);
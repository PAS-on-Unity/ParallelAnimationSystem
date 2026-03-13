using System.Runtime.InteropServices;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct Range(double Lower, double Upper);
using System.Runtime.InteropServices;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct SignedDistance(double Distance, double Dot);
using System.Runtime.InteropServices;
// ReSharper disable NotAccessedPositionalProperty.Global

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct MsdfVector2(double X, double Y);
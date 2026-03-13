using System.Runtime.InteropServices;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_edgeColoringSimple(IntPtr shape, double angleThreshold, ulong seed);

    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_edgeColoringInkTrap(IntPtr shape, double angleThreshold, ulong seed);

    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_edgeColoringByDistance(IntPtr shape, double angleThreshold, ulong seed);
}
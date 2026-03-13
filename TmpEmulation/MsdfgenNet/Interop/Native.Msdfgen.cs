using System.Runtime.InteropServices;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_generateSDF(IntPtr output, IntPtr shape, IntPtr transformation, IntPtr config);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_generatePSDF(IntPtr output, IntPtr shape, IntPtr transformation, IntPtr config);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_generateMSDF(IntPtr output, IntPtr shape, IntPtr transformation, IntPtr config);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_generateMTSDF(IntPtr output, IntPtr shape, IntPtr transformation, IntPtr config);
}
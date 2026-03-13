using System.Runtime.InteropServices;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_GeneratorConfig_create([MarshalAs(UnmanagedType.I1)] bool overlapSupport);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_GeneratorConfig_destroy(IntPtr config);
    
    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool msdfgen_GeneratorConfig_getOverlapSupport(IntPtr config);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_GeneratorConfig_setOverlapSupport(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool overlapSupport);
}
using System.Runtime.InteropServices;
using MsdfgenNet.Data;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_MSDFGeneratorConfig_create([MarshalAs(UnmanagedType.I1)] bool overlapSupport, IntPtr errorCorrectionConfig);
    
    [LibraryImport(LibraryName)]
    internal static partial ErrorCorrectionConfig msdfgen_MSDFGeneratorConfig_getErrorCorrectionConfig(IntPtr config);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_MSDFGeneratorConfig_setErrorCorrectionConfig(IntPtr config, IntPtr errorCorrectionConfig);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_MSDFGeneratorConfig_toBase(IntPtr config);
}
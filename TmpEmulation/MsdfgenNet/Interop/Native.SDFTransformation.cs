using System.Runtime.InteropServices;
using MsdfgenNet.Data;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_SDFTransformation_create();
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_SDFTransformation_createProjectionDistanceMapping(IntPtr projection, IntPtr distanceMapping);
    
    [LibraryImport(LibraryName)]
    internal static partial DistanceMapping msdfgen_SDFTransformation_getDistanceMapping(IntPtr transformation);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_SDFTransformation_setDistanceMapping(IntPtr transformation, IntPtr distanceMapping);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_SDFTransformation_toBase(IntPtr transformation);
}
using System.Runtime.InteropServices;
using MsdfgenNet.Data;
using Range = MsdfgenNet.Data.Range;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial DistanceMapping msdfgen_DistanceMapping_createRange(Range range);

    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_DistanceMapping_map(IntPtr distanceMapping, double d);
    
    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_DistanceMapping_mapDelta(IntPtr distanceMapping, double d);
    
    [LibraryImport(LibraryName)]
    internal static partial DistanceMapping msdfgen_DistanceMapping_inverse(IntPtr distanceMapping);
}
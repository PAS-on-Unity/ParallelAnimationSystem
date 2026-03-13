using System.Runtime.InteropServices;
using MsdfgenNet.Data;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_destroy(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_EdgeSegment_clone(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial EdgeType msdfgen_EdgeSegment_type(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial UIntPtr msdfgen_EdgeSegment_getControlPointsCount(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_getControlPoints(IntPtr edge, IntPtr points);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_EdgeSegment_point(IntPtr edge, double t);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_EdgeSegment_direction(IntPtr edge, double t);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_EdgeSegment_directionChange(IntPtr edge, double t);
    
    [LibraryImport(LibraryName)]
    internal static partial SignedDistance msdfgen_EdgeSegment_signedDistance(IntPtr edge, MsdfVector2 origin, IntPtr t);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_distanceToPerpendicularDistance(IntPtr edge, IntPtr distance, MsdfVector2 origin, double t);
    
    [LibraryImport(LibraryName)]
    internal static partial int msdfgen_EdgeSegment_scanlineIntersections(IntPtr edge, IntPtr x, IntPtr dy, double y);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_bound(IntPtr edge, IntPtr l, IntPtr b, IntPtr r, IntPtr t);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_reverse(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_moveStartPoint(IntPtr edge, MsdfVector2 to);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_moveEndPoint(IntPtr edge, MsdfVector2 to);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_splitInThirds(IntPtr edge, IntPtr part0, IntPtr part1, IntPtr part2);
    
    [LibraryImport(LibraryName)]
    internal static partial EdgeColor msdfgen_EdgeSegment_getEdgeColor(IntPtr edge);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeSegment_setEdgeColor(IntPtr edge, EdgeColor color);
}
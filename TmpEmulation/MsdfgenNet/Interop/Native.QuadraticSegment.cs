using System.Runtime.InteropServices;
using MsdfgenNet.Data;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_QuadraticSegment_create(MsdfVector2 p0, MsdfVector2 p1, MsdfVector2 p2, EdgeColor edgeColor);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_QuadraticSegment_convertToCubic(IntPtr segment);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_QuadraticSegment_toBase(IntPtr segment);
}
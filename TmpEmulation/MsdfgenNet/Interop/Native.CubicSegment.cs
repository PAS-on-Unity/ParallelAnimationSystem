using System.Runtime.InteropServices;
using MsdfgenNet.Data;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_CubicSegment_create(MsdfVector2 p0, MsdfVector2 p1, MsdfVector2 p2, MsdfVector2 p3, EdgeColor edgeColor);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_CubicSegment_toBase(IntPtr segment);
}
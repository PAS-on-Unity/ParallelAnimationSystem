using System.Runtime.InteropServices;
using MsdfgenNet.Data;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_LinearSegment_create(MsdfVector2 p0, MsdfVector2 p1, EdgeColor edgeColor);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_LinearSegment_toBase(IntPtr segment);
}
using System.Runtime.InteropServices;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_EdgeHolder_getSegment(IntPtr edgeHolder);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_EdgeHolder_setSegment(IntPtr edgeHolder, IntPtr segment);
}
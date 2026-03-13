using System.Runtime.InteropServices;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Contour_addEdge(IntPtr contour);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Contour_bound(IntPtr contour, IntPtr l, IntPtr b, IntPtr r, IntPtr t);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Contour_boundMiters(IntPtr contour, IntPtr l, IntPtr b, IntPtr r, IntPtr t, double border, double miterLimit, int polarity);
    
    [LibraryImport(LibraryName)]
    internal static partial int msdfgen_Contour_winding(IntPtr contour);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Contour_reverse(IntPtr contour);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Contour_createEdgesView(IntPtr contour);
}
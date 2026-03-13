using System.Runtime.InteropServices;
using MsdfgenNet.Data;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Shape_create();
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_destroy(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Shape_addContour(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_normalize(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool msdfgen_Shape_validate(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_bound(IntPtr shape, IntPtr l, IntPtr b, IntPtr r, IntPtr t);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_boundMiters(IntPtr shape, IntPtr l, IntPtr b, IntPtr r, IntPtr t, double border, double miterLimit, int polarity);
    
    [LibraryImport(LibraryName)]
    internal static partial Bounds msdfgen_Shape_getBounds(IntPtr shape, double border, double miterLimit, int polarity);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_scanline(IntPtr shape, IntPtr line, double y);
    
    [LibraryImport(LibraryName)]
    internal static partial int msdfgen_Shape_edgeCount(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_orientContours(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool msdfgen_Shape_getInverseYAxis(IntPtr shape);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Shape_setInverseYAxis(IntPtr shape, [MarshalAs(UnmanagedType.I1)] bool inverseYAxis);
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Shape_createContoursView(IntPtr shape);
}
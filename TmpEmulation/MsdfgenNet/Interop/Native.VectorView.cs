using System.Runtime.InteropServices;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Interop;

internal partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_VectorView_destroy(IntPtr vectorView);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_VectorView_data(IntPtr vectorView, IntPtr count, IntPtr data);
    
    [LibraryImport(LibraryName)]
    internal static partial UIntPtr msdfgen_VectorView_elementSize(IntPtr vectorView);
    
    [LibraryImport(LibraryName)]
    internal static partial UIntPtr msdfgen_VectorView_count(IntPtr vectorView);
    
    [LibraryImport(LibraryName)]
    internal static partial Error msdfgen_VectorView_at(IntPtr vectorView, UIntPtr index, IntPtr element);
    
    [LibraryImport(LibraryName)]
    internal static partial Error msdfgen_VectorView_set(IntPtr vectorView, UIntPtr index, IntPtr element);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_VectorView_add(IntPtr vectorView, IntPtr element);
    
    [LibraryImport(LibraryName)]
    internal static partial Error msdfgen_VectorView_insert(IntPtr vectorView, UIntPtr index, IntPtr element);
    
    [LibraryImport(LibraryName)]
    internal static partial Error msdfgen_VectorView_remove(IntPtr vectorView, UIntPtr index);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_VectorView_clear(IntPtr vectorView);
}
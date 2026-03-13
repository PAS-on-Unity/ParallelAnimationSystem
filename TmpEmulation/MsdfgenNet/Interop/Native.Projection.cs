using System.Runtime.InteropServices;
using MsdfgenNet.Data;

namespace MsdfgenNet.Interop;

internal static partial class Native
{
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Projection_create();
    
    [LibraryImport(LibraryName)]
    internal static partial IntPtr msdfgen_Projection_createRange(IntPtr scale, IntPtr translate);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Projection_destroy(IntPtr projection);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_project(IntPtr projection, IntPtr coord);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_unproject(IntPtr projection, IntPtr coord);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_projectVector(IntPtr projection, IntPtr vector);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_unprojectVector(IntPtr projection, IntPtr vector);
    
    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_Projection_projectX(IntPtr projection, double x);
    
    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_Projection_projectY(IntPtr projection, double y);
    
    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_Projection_unprojectX(IntPtr projection, double x);
    
    [LibraryImport(LibraryName)]
    internal static partial double msdfgen_Projection_unprojectY(IntPtr projection, double y);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_getScale(IntPtr projection);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Projection_setScale(IntPtr projection, IntPtr scale);
    
    [LibraryImport(LibraryName)]
    internal static partial MsdfVector2 msdfgen_Projection_getTranslate(IntPtr projection);
    
    [LibraryImport(LibraryName)]
    internal static partial void msdfgen_Projection_setTranslate(IntPtr projection, IntPtr translate);
}
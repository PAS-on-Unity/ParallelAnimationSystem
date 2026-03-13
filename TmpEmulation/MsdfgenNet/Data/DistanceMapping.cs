using System.Runtime.InteropServices;
using MsdfgenNet.Interop;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct DistanceMapping(double Scale, double Translate)
{
    public static DistanceMapping CreateRange(Range range) 
        => Native.msdfgen_DistanceMapping_createRange(range);

    public unsafe double Map(double d)
    {
        fixed (DistanceMapping* thisPtr = &this)
            return Native.msdfgen_DistanceMapping_map(new IntPtr(thisPtr), d);
    }
    
    public unsafe double MapDelta(double d)
    {
        fixed (DistanceMapping* thisPtr = &this)
            return Native.msdfgen_DistanceMapping_mapDelta(new IntPtr(thisPtr), d);
    }
    
    public unsafe DistanceMapping Inverse()
    {
        fixed (DistanceMapping* thisPtr = &this)
            return Native.msdfgen_DistanceMapping_inverse(new IntPtr(thisPtr));
    }
}
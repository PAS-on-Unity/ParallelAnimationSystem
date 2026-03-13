using MsdfgenNet.Data;
using MsdfgenNet.Enum;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class CubicSegment : EdgeSegment
{
    internal CubicSegment(IntPtr handle) : base(handle)
    {
    }
    
    public CubicSegment(MsdfVector2 p0, MsdfVector2 p1, MsdfVector2 p2, MsdfVector2 p3, EdgeColor edgeColor = EdgeColor.White) 
        : base(Native.msdfgen_CubicSegment_create(p0, p1, p2, p3, edgeColor))
    {
    }
}
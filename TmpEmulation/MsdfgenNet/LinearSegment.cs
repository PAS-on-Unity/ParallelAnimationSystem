using MsdfgenNet.Data;
using MsdfgenNet.Enum;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class LinearSegment : EdgeSegment
{
    internal LinearSegment(IntPtr handle) : base(handle)
    {
    }
    
    public LinearSegment(MsdfVector2 p0, MsdfVector2 p1, EdgeColor edgeColor = EdgeColor.White) 
        : base(Native.msdfgen_LinearSegment_create(p0, p1, edgeColor))
    {
    }
}
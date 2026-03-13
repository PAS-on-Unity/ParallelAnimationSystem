using MsdfgenNet.Data;
using MsdfgenNet.Enum;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class QuadraticSegment : EdgeSegment
{
    internal QuadraticSegment(IntPtr handle) : base(handle)
    {
    }
    
    public QuadraticSegment(MsdfVector2 p0, MsdfVector2 p1, MsdfVector2 p2, EdgeColor edgeColor = EdgeColor.White) 
        : base(Native.msdfgen_QuadraticSegment_create(p0, p1, p2, edgeColor))
    {
    }
    
    public CubicSegment ConvertToCubic() 
        => new(Native.msdfgen_QuadraticSegment_convertToCubic(Handle));
}
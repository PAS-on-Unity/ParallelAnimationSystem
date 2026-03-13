using MsdfgenNet.Enum;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

internal static class EdgeSegmentHelper
{
    internal static EdgeSegment CreateFromHandle(IntPtr handle)
    {
        var type = Native.msdfgen_EdgeSegment_type(handle);
        return type switch
        {
            EdgeType.Linear => new LinearSegment(handle),
            EdgeType.Quadratic => new QuadraticSegment(handle),
            EdgeType.Cubic => new CubicSegment(handle),
            _ => new EdgeSegment(handle),
        };
    }
}
using MsdfgenNet.Data;
using MsdfgenNet.Enum;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class EdgeSegment : NativeObject
{
    public EdgeType Type => Native.msdfgen_EdgeSegment_type(Handle);

    public EdgeColor Color
    {
        get => Native.msdfgen_EdgeSegment_getEdgeColor(Handle);
        set => Native.msdfgen_EdgeSegment_setEdgeColor(Handle, value);
    }
    
    public ReadOnlySpan<MsdfVector2> ControlPoints
    {
        get
        {
            if (controlPoints is null)
            {
                var count = Native.msdfgen_EdgeSegment_getControlPointsCount(Handle);
                var points = new MsdfVector2[count];
                unsafe
                {
                    fixed (MsdfVector2* pointsPtr = points)
                        Native.msdfgen_EdgeSegment_getControlPoints(Handle, new IntPtr(pointsPtr));
                }
                controlPoints = points;
            }
            return controlPoints;
        }
    }
    
    private MsdfVector2[]? controlPoints;
    
    internal EdgeSegment(IntPtr handle) : base(handle)
    {
    }

    public EdgeSegment Clone()
    {
        var handle = Native.msdfgen_EdgeSegment_clone(Handle);
        return EdgeSegmentHelper.CreateFromHandle(handle);
    }
    
    public MsdfVector2 Point(double t) 
        => Native.msdfgen_EdgeSegment_point(Handle, t);
    
    public MsdfVector2 Direction(double t) 
        => Native.msdfgen_EdgeSegment_direction(Handle, t);
    
    public MsdfVector2 DirectionChange(double t) 
        => Native.msdfgen_EdgeSegment_directionChange(Handle, t);
    
    public unsafe SignedDistance SignedDistance(MsdfVector2 origin, double t) 
        => Native.msdfgen_EdgeSegment_signedDistance(Handle, origin, new IntPtr(&t));

    public unsafe void DistanceToPerpendicularDistance(ref SignedDistance distance, MsdfVector2 origin, double t)
    {
        fixed (SignedDistance* distancePtr = &distance)
            Native.msdfgen_EdgeSegment_distanceToPerpendicularDistance(Handle, new IntPtr(distancePtr), origin, t);
    }

    public int ScanlineIntersections(Span<double> x, Span<double> dy, double y)
    {
        if (x.Length < 3)
            throw new ArgumentException("Span must have a length of at least 3", nameof(x));
        
        if (dy.Length < 3)
            throw new ArgumentException("Span must have a length of at least 3", nameof(dy));

        unsafe
        {
            fixed (double* xPtr = x)
            fixed (double* dyPtr = dy)
                return Native.msdfgen_EdgeSegment_scanlineIntersections(Handle, new IntPtr(xPtr), new IntPtr(dyPtr), y);
        }
    }
    
    public void Bound(out double l, out double b, out double r, out double t)
    {
        unsafe
        {
            fixed (double* lPtr = &l)
            fixed (double* bPtr = &b)
            fixed (double* rPtr = &r)
            fixed (double* tPtr = &t)
                Native.msdfgen_EdgeSegment_bound(Handle, new IntPtr(lPtr), new IntPtr(bPtr), new IntPtr(rPtr), new IntPtr(tPtr));
        }
    }
    
    public void Reverse() 
        => Native.msdfgen_EdgeSegment_reverse(Handle);
    
    public void MoveStartPoint(MsdfVector2 to)
        => Native.msdfgen_EdgeSegment_moveStartPoint(Handle, to);
    
    public void MoveEndPoint(MsdfVector2 to)
        => Native.msdfgen_EdgeSegment_moveEndPoint(Handle, to);

    public void SplitInThirds(out EdgeSegment part0, out EdgeSegment part1, out EdgeSegment part2)
    {
        var part0Handle = IntPtr.Zero;
        var part1Handle = IntPtr.Zero;
        var part2Handle = IntPtr.Zero;
        unsafe
        {
            Native.msdfgen_EdgeSegment_splitInThirds(Handle, new IntPtr(&part0Handle), new IntPtr(&part1Handle), new IntPtr(&part2Handle));
        }
        part0 = EdgeSegmentHelper.CreateFromHandle(part0Handle);
        part1 = EdgeSegmentHelper.CreateFromHandle(part1Handle);
        part2 = EdgeSegmentHelper.CreateFromHandle(part2Handle);
    }
    
    protected override void FreeHandle(IntPtr handle)
    {
        Native.msdfgen_EdgeSegment_destroy(handle);
    }
}
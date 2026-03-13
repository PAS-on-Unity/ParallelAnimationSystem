using MsdfgenNet.Data;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class Shape : NativeObject
{
    public IReadOnlyList<Contour> Contours
    {
        get
        {
            if (contours is null)
            {
                var ptr = Native.msdfgen_Shape_createContoursView(Handle);
                contours = new VectorView<Contour>(ptr, handle => new Contour(handle));
            }
            return contours;
        }
    }
    
    public bool InverseYAxis
    {
        get => Native.msdfgen_Shape_getInverseYAxis(Handle);
        set => Native.msdfgen_Shape_setInverseYAxis(Handle, value);
    }
    
    private VectorView<Contour>? contours;
    
    internal Shape(IntPtr handle) : base(handle)
    {
    }
    
    public Shape() : base(Native.msdfgen_Shape_create())
    {
    }
    
    public Contour AddContour()
    {
        var ptr = Native.msdfgen_Shape_addContour(Handle);
        return new Contour(ptr);
    }
    
    public void Normalize()
        => Native.msdfgen_Shape_normalize(Handle);
    
    public bool Validate()
        => Native.msdfgen_Shape_validate(Handle);
    
    public unsafe void Bound(out double l, out double b, out double r, out double t)
    {
        fixed (double* lPtr = &l, bPtr = &b, rPtr = &r, tPtr = &t)
            Native.msdfgen_Shape_bound(Handle, new IntPtr(lPtr), new IntPtr(bPtr), new IntPtr(rPtr), new IntPtr(tPtr));
    }
    
    public unsafe void BoundMiters(out double l, out double b, out double r, out double t, double border, double miterLimit, int polarity)
    {
        fixed (double* lPtr = &l, bPtr = &b, rPtr = &r, tPtr = &t)
            Native.msdfgen_Shape_boundMiters(Handle, new IntPtr(lPtr), new IntPtr(bPtr), new IntPtr(rPtr), new IntPtr(tPtr), border, miterLimit, polarity);
    }
    
    public Bounds GetBounds(double border, double miterLimit, int polarity)
        => Native.msdfgen_Shape_getBounds(Handle, border, miterLimit, polarity);
    
    public int EdgeCount()
        => Native.msdfgen_Shape_edgeCount(Handle);
    
    public void OrientContours()
        => Native.msdfgen_Shape_orientContours(Handle);

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        contours?.Dispose();
    }

    protected override void FreeHandle(IntPtr handle)
    {
        Native.msdfgen_Shape_destroy(handle);
    }
}
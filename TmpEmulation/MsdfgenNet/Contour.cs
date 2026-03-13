using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class Contour : NativeObject
{
    public IReadOnlyList<EdgeHolder> Edges
    {
        get
        {
            if (edges is null)
            {
                var ptr = Native.msdfgen_Contour_createEdgesView(Handle);
                edges = new VectorView<EdgeHolder>(ptr, handle => new EdgeHolder(handle));
            }
            return edges;
        }
    }

    private VectorView<EdgeHolder>? edges;
    
    internal Contour(IntPtr handle) : base(handle)
    {
    }
    
    public EdgeHolder AddEdge()
    {
        var ptr = Native.msdfgen_Contour_addEdge(Handle);
        return new EdgeHolder(ptr);
    }
    
    public unsafe void Bound(out double l, out double b, out double r, out double t)
    {
        fixed (double* lPtr = &l, bPtr = &b, rPtr = &r, tPtr = &t)
            Native.msdfgen_Contour_bound(Handle, new IntPtr(lPtr), new IntPtr(bPtr), new IntPtr(rPtr), new IntPtr(tPtr));
    }
    
    public unsafe void BoundMiters(out double l, out double b, out double r, out double t, double border, double miterLimit, int polarity)
    {
        fixed (double* lPtr = &l, bPtr = &b, rPtr = &r, tPtr = &t)
            Native.msdfgen_Contour_boundMiters(Handle, new IntPtr(lPtr), new IntPtr(bPtr), new IntPtr(rPtr), new IntPtr(tPtr), border, miterLimit, polarity);
    }
    
    public int Winding()
        => Native.msdfgen_Contour_winding(Handle);
    
    public void Reverse()
        => Native.msdfgen_Contour_reverse(Handle);

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        edges?.Dispose();
    }

    protected override void FreeHandle(IntPtr handle)
    {
        // Do nothing; this object does not own the handle
    }
}
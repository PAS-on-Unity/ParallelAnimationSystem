using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class EdgeHolder : NativeObject
{
    public EdgeSegment? Segment
    {
        get
        {
            var ptr = Native.msdfgen_EdgeHolder_getSegment(Handle);
            return ptr == IntPtr.Zero ? null : EdgeSegmentHelper.CreateFromHandle(ptr);
        }
        set => Native.msdfgen_EdgeHolder_setSegment(Handle, value?.Handle ?? IntPtr.Zero);
    }
    
    internal EdgeHolder(IntPtr handle) : base(handle)
    {
    }
    
    protected override void FreeHandle(IntPtr handle)
    {
        // Do nothing; this object does not own the handle
    }
}
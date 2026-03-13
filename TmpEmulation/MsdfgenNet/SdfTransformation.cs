using MsdfgenNet.Data;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class SdfTransformation : Projection
{
    public unsafe DistanceMapping DistanceMapping
    {
        get => Native.msdfgen_SDFTransformation_getDistanceMapping(Handle);
        set => Native.msdfgen_SDFTransformation_setDistanceMapping(Handle, new IntPtr(&value));
    }
    
    internal SdfTransformation(IntPtr handle) : base(handle)
    {
    }
    
    public SdfTransformation() 
        : base(Native.msdfgen_SDFTransformation_create())
    {
    }
    
    public unsafe SdfTransformation(Projection projection, DistanceMapping distanceMapping) 
        : base(Native.msdfgen_SDFTransformation_createProjectionDistanceMapping(projection.Handle, new IntPtr(&distanceMapping)))
    {
    }
}
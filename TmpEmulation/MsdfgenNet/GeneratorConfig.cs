using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class GeneratorConfig : NativeObject
{
    public bool OverlapSupport
    {
        get => Native.msdfgen_GeneratorConfig_getOverlapSupport(Handle);
        set => Native.msdfgen_GeneratorConfig_setOverlapSupport(Handle, value);
    }
    
    internal GeneratorConfig(IntPtr handle) : base(handle)
    {
    }
    
    public GeneratorConfig(bool overlapSupport) : this(Native.msdfgen_GeneratorConfig_create(overlapSupport))
    {
    }
    
    protected override void FreeHandle(IntPtr handle)
    {
        Native.msdfgen_GeneratorConfig_destroy(handle);
    }
}
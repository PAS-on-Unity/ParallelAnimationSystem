using MsdfgenNet.Data;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public class MsdfGeneratorConfig : GeneratorConfig
{
    public unsafe ErrorCorrectionConfig ErrorCorrectionConfig
    {
        get => Native.msdfgen_MSDFGeneratorConfig_getErrorCorrectionConfig(Handle);
        set => Native.msdfgen_MSDFGeneratorConfig_setErrorCorrectionConfig(Handle, new IntPtr(&value));
    }
    
    internal MsdfGeneratorConfig(IntPtr handle) : base(handle)
    {
    }
    
    public unsafe MsdfGeneratorConfig(bool overlapSupport, ErrorCorrectionConfig errorCorrectionConfig) 
        : this(Native.msdfgen_MSDFGeneratorConfig_create(overlapSupport, new IntPtr(&errorCorrectionConfig)))
    {
    }
}
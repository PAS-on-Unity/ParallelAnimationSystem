using System.Runtime.CompilerServices;
using MsdfgenNet.Data;
using MsdfgenNet.Interop;

[assembly: DisableRuntimeMarshalling]

namespace MsdfgenNet;

public static class Msdfgen
{
    public static unsafe void GenerateSdf(Bitmap bitmap, Shape shape, SdfTransformation transformation, GeneratorConfig generatorConfig)
    {
        if (bitmap.Channels != 1)
            throw new ArgumentException("Bitmap must have 1 channel", nameof(bitmap));

        fixed (float* dataPtr = bitmap.Data)
        {
            var bitmapRef = new BitmapRef(new IntPtr(dataPtr), bitmap.Width, bitmap.Height);
            Native.msdfgen_generateSDF(new IntPtr(&bitmapRef), shape.Handle, transformation.Handle, generatorConfig.Handle);
        }
    }
    
    public static unsafe void GeneratePsdf(Bitmap bitmap, Shape shape, SdfTransformation transformation, GeneratorConfig generatorConfig)
    {
        if (bitmap.Channels != 1)
            throw new ArgumentException("Bitmap must have 1 channel", nameof(bitmap));

        fixed (float* dataPtr = bitmap.Data)
        {
            var bitmapRef = new BitmapRef(new IntPtr(dataPtr), bitmap.Width, bitmap.Height);
            Native.msdfgen_generatePSDF(new IntPtr(&bitmapRef), shape.Handle, transformation.Handle, generatorConfig.Handle);
        }
    }
    
    public static unsafe void GenerateMsdf(Bitmap bitmap, Shape shape, SdfTransformation transformation, MsdfGeneratorConfig generatorConfig)
    {
        if (bitmap.Channels != 3)
            throw new ArgumentException("Bitmap must have 3 channels", nameof(bitmap));

        fixed (float* dataPtr = bitmap.Data)
        {
            var bitmapRef = new BitmapRef(new IntPtr(dataPtr), bitmap.Width, bitmap.Height);
            Native.msdfgen_generateMSDF(new IntPtr(&bitmapRef), shape.Handle, transformation.Handle, generatorConfig.Handle);
        }
    }
    
    public static unsafe void GenerateMtsdf(Bitmap bitmap, Shape shape, SdfTransformation transformation, MsdfGeneratorConfig generatorConfig)
    {
        if (bitmap.Channels != 4)
            throw new ArgumentException("Bitmap must have 4 channels", nameof(bitmap));

        fixed (float* dataPtr = bitmap.Data)
        {
            var bitmapRef = new BitmapRef(new IntPtr(dataPtr), bitmap.Width, bitmap.Height);
            Native.msdfgen_generateMTSDF(new IntPtr(&bitmapRef), shape.Handle, transformation.Handle, generatorConfig.Handle);
        }
    }
}

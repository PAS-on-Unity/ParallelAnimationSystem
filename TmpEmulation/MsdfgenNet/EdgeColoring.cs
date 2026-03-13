using MsdfgenNet.Interop;

namespace MsdfgenNet;

public static class EdgeColoring
{
    public static void ColorSimple(Shape shape, double angleThreshold, ulong seed = 0ul)
    {
        Native.msdfgen_edgeColoringSimple(shape.Handle, angleThreshold, seed);
    }
    
    public static void ColorInkTrap(Shape shape, double angleThreshold, ulong seed = 0ul)
    {
        Native.msdfgen_edgeColoringInkTrap(shape.Handle, angleThreshold, seed);
    }
    
    public static void ColorByDistance(Shape shape, double angleThreshold, ulong seed = 0ul)
    {
        Native.msdfgen_edgeColoringByDistance(shape.Handle, angleThreshold, seed);
    }
}
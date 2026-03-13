using System.Runtime.InteropServices;
using MsdfgenNet.Enum;

namespace MsdfgenNet.Data;

[StructLayout(LayoutKind.Sequential)]
public record struct ErrorCorrectionConfig(
    ErrorCorrectionConfigMode Mode,
    ErrorCorrectionConfigDistanceCheckMode DistanceCheckMode,
    double MinDeviationRatio,
    double MinImproveRatio,
    IntPtr Buffer = default);
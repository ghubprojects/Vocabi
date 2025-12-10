namespace Vocabi.Application.Common.Configurations;

public class PerformanceSettings
{
    public int ThresholdMilliseconds { get; set; } = 500;
    public bool EnableTracing { get; set; }
}
using Solid.Domain.Enums;

namespace Solid.Domain.Models;

public class Sensor
{
    public Guid Id { get; set; }
    public float? Tcf { get; set; }
    public string FwVersion { get; set; }
    public SensorType? Type { get; set; }
    public string FileName { get; set; }
    public string? DeviceId { get; set; }
}

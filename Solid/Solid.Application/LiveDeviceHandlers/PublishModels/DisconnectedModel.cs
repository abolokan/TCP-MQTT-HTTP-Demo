namespace Solid.Application.LiveDeviceHandlers.PublishModels;

public class DisconnectedModel
{
    public string Time { get; set; }
    public string E => "offline";
    public string DeviceId { get; set; }
}

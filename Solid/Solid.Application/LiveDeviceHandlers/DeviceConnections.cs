using Solid.Common.Exceptions;
using System.Net;

namespace Solid.Application.LiveDeviceHandlers;

public class DeviceConnections
{
    private DeviceConnections()
    {

    }

    private static List<LiveDevice> connectedDevices = [];

    public static LiveDevice TryGetDevice(string deviceId)
    {
        var device = connectedDevices.FirstOrDefault(c => c.DeviceId == deviceId);

        return device is null ? throw new ApiException($"Device '${deviceId}' is not connected", HttpStatusCode.NotFound) : device;
    }

    public static LiveDevice AddDevice(LiveDevice device)
    {
        connectedDevices.Add(device);
        return device;
    }

    public static bool TryRemoveDevice(string deviceId)
    {
        var device = connectedDevices.FirstOrDefault(c => c.DeviceId == deviceId);
        if (device == null)
            return false;

        return connectedDevices.Remove(device);
    }
}

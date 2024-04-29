using Microsoft.Extensions.Logging;
using Solid.Application.Interfaces;
using Solid.MQTT;
using System.Net.Sockets;

namespace Solid.Application.LiveDeviceHandlers;

public class LiveDeviceFactory : ILiveDeviceFactory
{
    private readonly IMqttClientWrapper _mqttClientWrapper;
    private readonly ILogger<LiveDevice> _liveDeviceLogger;
    private readonly ISensorService _sensorService;
    private readonly IFileLogger _fileLogger;

    public LiveDeviceFactory(
        IMqttClientWrapper mqttClientWrapper,
        ILogger<LiveDevice> liveDeviceLogger,
        IFileLogger fileLogger,
        ISensorService sensorService)
    {
        _mqttClientWrapper = mqttClientWrapper;
        _liveDeviceLogger = liveDeviceLogger;
        _fileLogger = fileLogger;
        _sensorService = sensorService;
    }

    public LiveDevice CreateLiveDevice(TcpClient tcpClient)
    {
        return new LiveDevice(tcpClient)
            .AddPublishClient(_mqttClientWrapper)
            .AddFileLogger(_fileLogger)
            .AddLogger(_liveDeviceLogger)
            .AddSensorService(_sensorService);
    }
}
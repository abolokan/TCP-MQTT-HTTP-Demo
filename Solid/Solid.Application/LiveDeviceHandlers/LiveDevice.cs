using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solid.Application.Interfaces;
using Solid.Application.LiveDeviceHandlers.PublishModels;
using Solid.Application.Messages;
using Solid.MQTT;
using System.Net.Sockets;
using System.Text;

namespace Solid.Application.LiveDeviceHandlers;

public class LiveDevice
{
    public string DeviceId => _deviceId;

    private string _deviceId;

    private IMqttClientWrapper _mqttClient;
    private IFileLogger _fileLogger;
    private ILogger<LiveDevice> _logger;
    private ISensorService _sensorService;

    private readonly TcpClient _tcpClient;
    private readonly StreamWriter writer;

    private const int clientTimeout = 1000 * 60 * 5; // 5 mins

    public LiveDevice(TcpClient tcpClient)
    {
        _tcpClient = tcpClient;

        _deviceId = _tcpClient.Client.Handle.ToString();

        //_tcpClient.NoDelay = true;
        //_tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

        writer = new StreamWriter(_tcpClient.GetStream(), Encoding.UTF8);

        // Set timeout
        // Task.Run(() => CheckSocketTimeout(_tcpClient));
    }

    public bool IsConnected => _tcpClient.Connected;

    public void SendCommand(string commandstring)
    {
        if (string.IsNullOrEmpty(commandstring))
            throw new ArgumentException("Command string cannot be null or empty");

        if (!IsConnected)
            throw new InvalidOperationException("Device is not connected.");

        writer.WriteLine($"\n{commandstring.Trim()}\n");
        writer.Flush();
    }

    public void RequestUpdate()
    {
        SendCommand("u");
    }

    private void CheckSocketTimeout(TcpClient client)
    {
        Thread.Sleep(clientTimeout);
        if (client.Connected)
        {
            _logger.LogWarning($"Socket timeout for {DeviceId}");

            DisconnectedAsync().GetAwaiter().GetResult();
        }
    }

    public async Task DataAsync(TcpDataMessage data, CancellationToken cancellationToken)
    {
        try
        {
            // Check for correctable values
            if (data.Temp != null)
            {
                // Fetch device with all its data
                var device = await _sensorService.GetByDeviceIdAsync(data.DeviceId, cancellationToken);

                // Verify device
                if (device != null)
                {
                    // Only apply correction factor if there is any.
                    if (device.Tcf.HasValue)
                    {
                        // Backup old
                        data.TempBackup = data.Temp;

                        // Save new
                        data.Temp = Math.Round(data.Temp.Value * device.Tcf.Value);
                    }
                }
            }
            else if (data.TempC.HasValue) // Already compensated temperature
            {
                data.Temp = Math.Round(data.TempC.Value); // store already compensated temperature as temp
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while attempting to autocorrect values:");
        }

        await PublishSubDataAsync(data);

        // Log event
        var dataString = JsonConvert.SerializeObject(data);
        _fileLogger.Write(dataString);
        _fileLogger.Write("\n\r");

        // Remove duplicate device_id already present in topic
        data.DeviceId = string.Empty;

        await _mqttClient.PublishMessageAsync($"sensor/{DeviceId}/data", JsonConvert.SerializeObject(data), true);
    }

    /// <summary>
    /// // Receives raw data packets, like HTTP get commands, or _printf data (non-json) from sensors. Often contains data from people trying to break in
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public async Task RawAsync(object raw)
    {
        //Added check on device_id, so malicious servers can't send mqtt messages.
        if (!_deviceId.Contains("Unknown"))
        {
            await _mqttClient.PublishMessageAsync($"sensor/{DeviceId}/raw", JsonConvert.SerializeObject(raw));
        }
    }
    public void ChangeDeviceId(object newDeviceId)
    {
        if (newDeviceId != null)
        {
            var id = newDeviceId.ToString();
            _logger.LogInformation($"Device '{_deviceId}' is now known as '{id}'");
            _deviceId = id;
        }
    }
    public async Task OnlineAsync(TcpOnlineMessage data)
    {
        await _mqttClient.PublishMessageAsync($"sensor/{DeviceId}/online", JsonConvert.SerializeObject(data));

        // Log event
        var newData = data as dynamic;
        newData.e = "online";
        _fileLogger.Write(JsonConvert.SerializeObject(newData));
        _fileLogger.Write("\n\r");
    }
    public async Task ErrorAsync(object error)
    {
        await _mqttClient.PublishMessageAsync($"sensor/{DeviceId}/error", JsonConvert.SerializeObject(error));
    }

    private async Task PublishSubDataAsync(TcpDataMessage data)
    {
        try
        {
            // If the data has "subdata", meaning that it has slave-device data
            if (data.Sub != null)
            {
                await _mqttClient.PublishMessageAsync($"sensor/{data.DeviceId}/subdata", JsonConvert.SerializeObject(data), retain: true); // TODO: should we really retain?
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while attempting to send sub-data: ");
        }
    }

    /// <summary>
    /// // Remove the client from the list when it leaves
    /// </summary>
    /// <returns></returns>
    public async Task DisconnectedAsync()
    {
        _tcpClient.Close();

        var removed = DeviceConnections.TryRemoveDevice(DeviceId);
        if (!removed)
            return;

        var data = new DisconnectedModel
        {
            Time = DateTime.UtcNow.ToString("yyyy-mm-dd HH:MM:ss")
        };

        await _mqttClient.PublishMessageAsync($"sensor/{DeviceId}/offline", JsonConvert.SerializeObject(data));

        data.DeviceId = DeviceId;

        // Log event
        _logger.LogInformation($"Device {DeviceId} disconnected...");

        _fileLogger.Write(JsonConvert.SerializeObject(data));
        _fileLogger.Write("\n\r");
    }

    #region Builder methods

    internal LiveDevice AddPublishClient(IMqttClientWrapper mqttClientWrapper)
    {
        this._mqttClient = mqttClientWrapper;
        return this;
    }

    internal LiveDevice AddFileLogger(IFileLogger fileLogger)
    {
        this._fileLogger = fileLogger;
        return this;
    }

    internal LiveDevice AddLogger(ILogger<LiveDevice> liveDeviceLogger)
    {
        this._logger = liveDeviceLogger;
        return this;
    }
    internal LiveDevice AddSensorService(ISensorService sensorService)
    {
        this._sensorService = sensorService;
        return this;
    }

    #endregion
}
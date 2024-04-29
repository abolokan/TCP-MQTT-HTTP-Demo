using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Net;
using Solid.Application.LiveDeviceHandlers;
using System.Text;
using Solid.Infrastructure.Interfaces;
using Solid.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Solid.Infrastructure;

public class TcpServerOptions
{
    public const string Section = "TcpServerOptions";

    public string Address { get; set; } = null!;
    public int Port { get; set; }
}

public class TcpServerService : ITcpServerService
{
    private readonly ILogger<TcpServerService> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ILiveDeviceFactory _liveDeviceFactory;
    private readonly TcpServerOptions _options;

    public TcpServerService(
        IOptions<TcpServerOptions> options,
        ILogger<TcpServerService> logger,
        IMessageHandler messageHandler,
        ILiveDeviceFactory liveDeviceFactory)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _liveDeviceFactory = liveDeviceFactory;
        _options = options.Value;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        IPAddress localAddress = IPAddress.Parse(_options.Address);
        TcpListener listener = new TcpListener(localAddress, _options.Port);

        try
        {
            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = ProcessClientAsync(client);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in TCP server: {ex.Message}");
        }
        finally
        {
            listener.Stop();
            listener.Dispose();
        }
    }

    private async Task ProcessClientAsync(TcpClient tcpClient)
    {
        var device = _liveDeviceFactory.CreateLiveDevice(tcpClient);

        DeviceConnections.AddDevice(device);

        var stream = tcpClient.GetStream();

        try
        {
            while (tcpClient.Connected)
            {
                byte[] msg = new byte[1024];
                stream.Read(msg, 0, msg.Length);
                string receivedMessage = Encoding.UTF8.GetString(msg);
                await _messageHandler.ReceiveDeviceMessageAsync(device, receivedMessage, CancellationToken.None);
            }
        }
        catch (IOException ioExc)
        {
            _logger.LogError(ioExc, ioExc.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
        finally
        {
            await device.DisconnectedAsync();
        }
    }
}

using Solid.Application.Interfaces;
using Solid.Application.LiveDeviceHandlers;

namespace Solid.Application.Messages;

public class MessageHandler : IMessageHandler
{
    private readonly IParser _parser;

    public MessageHandler(IParser parser)
    {
        _parser = parser;
    }

    public async Task ReceiveDeviceMessageAsync(LiveDevice device, string receivedMessage, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(receivedMessage)) return;

        var tcpMessage = _parser.Parse<TcpBaseMessage>(receivedMessage);

        if (tcpMessage?.Data == null) return;

        switch (tcpMessage.Action)
        {
            case "data":
                var dataMessage = _parser.Parse<TcpBaseMessage<TcpDataMessage>>(receivedMessage);
                await device.DataAsync(dataMessage.Data, cancellationToken);
                break;
            case "raw":
                await device.RawAsync(tcpMessage.Data);
                break;
            case "device_id":
                device.ChangeDeviceId(tcpMessage.Data);
                break;
            case "online":
                var onlineMessage = _parser.Parse<TcpBaseMessage<TcpOnlineMessage>>(receivedMessage);
                await device.OnlineAsync(onlineMessage.Data);
                break;
            case "error":
                await device.ErrorAsync(tcpMessage.Data);
                break;
            case "disconnected":
                await device.DisconnectedAsync();
                break;
            default:
                throw new NotImplementedException(tcpMessage.Action);
        }
    }
}

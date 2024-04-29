using Solid.Application.LiveDeviceHandlers;

namespace Solid.Application.Interfaces;

public interface IMessageHandler
{
    Task ReceiveDeviceMessageAsync(LiveDevice device, string receivedMessage, CancellationToken cancellationToken);
}

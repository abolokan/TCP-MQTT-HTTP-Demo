using System.Net.Sockets;

namespace Solid.Application.LiveDeviceHandlers;

public interface ILiveDeviceFactory
{
    LiveDevice CreateLiveDevice(TcpClient tcpClient);
}



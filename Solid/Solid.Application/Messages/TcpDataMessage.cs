namespace Solid.Application.Messages;

public class TcpDataMessage
{
    public double? Temp { get; set; }
    public double? TempBackup { get; set; }
    public double? TempC { get; set; }
    public string DeviceId { get; set; }
    public TcpDataMessage Sub { get; set; }

}

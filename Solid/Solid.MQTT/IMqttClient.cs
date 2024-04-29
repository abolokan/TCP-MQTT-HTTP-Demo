namespace Solid.MQTT
{
    public interface IMqttClientWrapper
    {
        Task ConnectAsync(string address, int port);
        Task PublishMessageAsync(string topic, string payload, bool retain = false);
    }
}

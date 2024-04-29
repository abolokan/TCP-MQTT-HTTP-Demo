namespace Solid.MQTT
{
    public class MqttClientConfiguration
    {
        public const string Section = "MqttClientConfiguration";
        public string Address { get; set; }
        public int Port { get; set; }
        public string[] Subscriptions { get; set; } = [];
    }
}

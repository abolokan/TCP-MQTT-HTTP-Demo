using MQTTnet;
using MQTTnet.Server;

class Program
{
    static async Task Main()
    {
        var optionsBuilder = new MqttServerOptionsBuilder()
            .WithDefaultEndpoint()
            .WithDefaultEndpointPort(1883);

        // Start the MQTT server
        var mqttServerOptions = optionsBuilder.Build();

        // Create a new MQTT server instance
        var mqttServer = new MqttFactory().CreateMqttServer(mqttServerOptions);

        await mqttServer.StartAsync();

        Console.WriteLine("MQTT server started. Press any key to exit.");
        Console.ReadKey();

        // Stop the MQTT server
        await mqttServer.StopAsync();
    }
}

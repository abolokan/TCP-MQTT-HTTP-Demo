using MQTTnet.Client;
using MQTTnet;
using System.Text;
using MQTTnet.Server;

namespace MqttClientApp;

public class MqttClientWrapper
{
    public event EventHandler<string> OnReceivedMessage;
    public event EventHandler OnConnected;
    public event EventHandler OnDisConnected;

    private IMqttClient _mqttClient;
    private MqttFactory _mqttFactory;
    private readonly string[] _subscriptions;

    public MqttClientWrapper(string[] subscriptions)
    {
        _subscriptions = subscriptions;
        InitAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public async Task ConnectAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("127.0.0.1", 1883)
            .Build();

        await _mqttClient.ConnectAsync(options);

        if (_mqttClient.IsConnected)
        {
            await SubscribeAsync();
            HandlerRegistration();
        }
    }

    public async Task<MqttClientPublishResult> PublishMessageAsync(string topic, string payload, bool retain = false)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithRetainFlag(retain)
            .Build();

        return await _mqttClient.PublishAsync(message);
    }

    public async Task DisconnectAsync()
    {
        await _mqttClient.DisconnectAsync();
    }
    private async Task InitAsync()
    {
        _mqttFactory = new MqttFactory();
        _mqttClient = _mqttFactory.CreateMqttClient();
    }

    private async Task SubscribeAsync()
    {
        var builder = _mqttFactory.CreateSubscribeOptionsBuilder();

        foreach (var subscription in _subscriptions)
        {
            builder = builder.WithTopicFilter(
                      f =>
                      {
                          f.WithTopic(subscription);
                      });
        }

        var mqttSubscribeOptions = builder.Build();
        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
    }

    private void HandlerRegistration()
    {
        // Setup message handling before connecting so that queued messages
        // are also handled properly. When there is no event handler attached all
        // received messages get lost.
        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            if (OnReceivedMessage != null)
            {
                OnReceivedMessage(this, payload);
            }

            return Task.CompletedTask;
        };

        _mqttClient.ConnectedAsync += e =>
        {
            if (OnConnected != null)
            {
                OnConnected(this, e);
            }

            return Task.CompletedTask;
        };

        _mqttClient.DisconnectedAsync += e =>
        {
            if (OnDisConnected != null)
            {
                OnDisConnected(this, e);
            }

            return Task.CompletedTask;
        };
    }
}

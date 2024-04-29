using MQTTnet.Client;
using MQTTnet;
using System.Text;
using MQTTnet.Server;
using Microsoft.Extensions.Options;

namespace Solid.MQTT;

public class MqttClientWrapper : IMqttClientWrapper
{
    public event EventHandler<string> OnReceivedMessage;
    public event EventHandler OnConnected;
    public event EventHandler OnDisConnected;

    private IMqttClient _mqttClient;
    private MqttFactory _mqttFactory;
    private readonly MqttClientConfiguration _clientConfig;

    public MqttClientWrapper(IOptions<MqttClientConfiguration> options)
    {
        _clientConfig = options.Value;
        InitAsync().Wait();
    }

    public async Task ConnectAsync(string address, int port)
    {
        if (_mqttClient.IsConnected)
        {
            return;
        }

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(address, port)
            .Build();

        await _mqttClient.ConnectAsync(options);

        if (_mqttClient.IsConnected)
        {
            await SubscribeAsync();
            HandlerRegistration();
        }
    }

    /// <summary>
    /// Publish event
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="payload"></param>
    /// <param name="retain"></param>
    /// <returns></returns>
    public async Task PublishMessageAsync(string topic, string payload, bool retain = false)
    {
        if (_mqttClient.IsConnected)
        {
            var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithRetainFlag(retain)
            .Build();

            await _mqttClient.PublishAsync(message);
        }
    }

    public async Task DisconnectAsync()
    {
        await _mqttClient.DisconnectAsync();
    }

    private async Task InitAsync()
    {
        _mqttFactory = new MqttFactory();
        _mqttClient = _mqttFactory.CreateMqttClient();
        await ConnectAsync(_clientConfig.Address, _clientConfig.Port);
    }

    private async Task SubscribeAsync()
    {
        var builder = _mqttFactory.CreateSubscribeOptionsBuilder();

        foreach (var subscription in _clientConfig.Subscriptions)
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
            // TODO:confirm and check
            /*
             mqtt.on('connect', function(connack) {
         if (!connack.sessionPresent) {
             console.log('[MQTT] Connected to local MQTT server'.green);

             mqtt.subscribe("sensor/+/command");
         }
         else {
             console.log('[MQTT] *Reconnected* to local MQTT server'.green);
             mqtt.subscribe("sensor/+/command");
         }
         });
             */

            //if (!e.ConnectResult.IsSessionPresent)
            //{
            //    await SubscribeAsync();
            //}
            //else
            //{
            //    await SubscribeAsync();
            //}

            if (OnConnected != null)
            {
                OnConnected(this, e);
            }

            return Task.CompletedTask;
        };

        _mqttClient.DisconnectedAsync += e =>
        {

            if (OnConnected != null)
            {
                OnConnected(this, e);
            }

            if (OnDisConnected != null)
            {
                OnDisConnected(this, e);
            }

            return Task.CompletedTask;
        };
    }
}

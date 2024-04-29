using Microsoft.Extensions.DependencyInjection;
using Solid.Application.Interfaces;
using Solid.Application.LiveDeviceHandlers;
using Solid.Application.Messages;
using Solid.MQTT;

namespace Solid.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IParser, ParserService>();
        services.AddSingleton<IMqttClientWrapper, MqttClientWrapper>();
        services.AddSingleton<IFileLogger, FileLogger>();
        services.AddSingleton<IMessageHandler, MessageHandler>();

        services.AddScoped<ILiveDeviceFactory, LiveDeviceFactory>();
        services.AddScoped<IFileManager, FileManager>();

        services.AddTransient<ISensorService, SensorService>();

        return services;
    }
}
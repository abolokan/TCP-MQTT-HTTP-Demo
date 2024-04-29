using Solid.Application;
using Solid.Infrastructure;
using Solid.Infrastructure.Interfaces;
using Solid.MQTT;
using Solid.Persistence;
using Solid.WebAPI.AppSettings;
using Solid.WebAPI.Common;

namespace Solid.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configBuilder = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var configuration = configBuilder.Build();
        builder.Configuration.Bind(configuration);

        builder.Services.Configure<TcpServerOptions>(builder.Configuration.GetSection(TcpServerOptions.Section));
        builder.Services.Configure<MqttClientConfiguration>(builder.Configuration.GetSection(MqttClientConfiguration.Section));
        builder.Services.Configure<RequiredHtppHeaders>(builder.Configuration.GetSection(RequiredHtppHeaders.Section));

        // Add services to the container.
        builder.Services.AddPersistence(configuration);
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        Task.Factory.StartNew(() => RunTcpServer(app));

        app.UseCustomExceptionHandler();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }

    private static void RunTcpServer(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<ITcpServerService>();
            service.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}

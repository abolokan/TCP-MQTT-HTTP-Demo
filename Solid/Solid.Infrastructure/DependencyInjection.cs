using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solid.Application;
using Solid.Application.Interfaces;
using Solid.Infrastructure.Interfaces;
using System.IO.Abstractions;

namespace Solid.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITcpServerService, TcpServerService>();
        services.AddScoped<ICryptoService, CryptoService>();
        services.AddScoped<IFileSystem, FileSystem>();

        return services;
    }
}
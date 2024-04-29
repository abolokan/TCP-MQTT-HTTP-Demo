namespace Solid.Infrastructure.Interfaces;

public interface ITcpServerService
{
    Task ExecuteAsync(CancellationToken stoppingToken);
}

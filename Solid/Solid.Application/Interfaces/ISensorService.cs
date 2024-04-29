using Solid.Domain.Models;

namespace Solid.Application.Interfaces;

public interface ISensorService
{
    public Task<Sensor> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken);
}

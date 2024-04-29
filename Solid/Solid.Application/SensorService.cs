using Microsoft.EntityFrameworkCore;
using Solid.Application.Interfaces;
using Solid.Domain.Models;
using Solid.Persistence;

namespace Solid.Application;

public class SensorService : ISensorService
{
    private readonly IAppDbContext _appDbContext;

    public SensorService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Sensor> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken)
    {
        return await _appDbContext.Sensors
            .Where(x => x.DeviceId == deviceId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

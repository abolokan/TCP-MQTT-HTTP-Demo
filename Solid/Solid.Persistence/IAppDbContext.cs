using Microsoft.EntityFrameworkCore;
using Solid.Domain.Models;

namespace Solid.Persistence;

public interface IAppDbContext:IDisposable
{
    DbSet<Sensor> Sensors { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

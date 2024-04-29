using Microsoft.EntityFrameworkCore;
using Solid.Domain.Models;

namespace Solid.Persistence;

public partial class AppDbContext : IAppDbContext
{
    public DbSet<Sensor> Sensors { get; set; }
}

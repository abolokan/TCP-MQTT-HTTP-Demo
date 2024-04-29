using Microsoft.AspNetCore.Mvc;
using Solid.Data.Generator;
using Solid.Persistence;

namespace Solid.WebAPI.Controllers;

[ApiController]
[Route("test-data")]
public class TestDataController : ControllerBase
{
    private readonly IAppDbContext _appDbContext;

    public TestDataController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpPost("generate/sensors")]
    public async Task<ActionResult> GenerateSensorsAsync(CancellationToken cancellationToken)
    {
        await _appDbContext.Sensors.AddRangeAsync(SensorDataGenerator.SensorFakeData(), cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }
}

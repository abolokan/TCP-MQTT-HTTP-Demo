using Bogus;
using Solid.Domain.Enums;
using Solid.Domain.Models;

namespace Solid.Data.Generator;

public class SensorDataGenerator
{
    public static List<Sensor> SensorFakeData()
    {
        var faker = new Faker<Sensor>()
           .RuleFor(x => x.Id, f => f.Random.Guid())
           .RuleFor(x => x.Tcf, f => f.Random.Float(100, 1000))
           .RuleFor(x => x.FwVersion, f => $"{f.Random.Number(1, 10)}.{f.Random.Number(0, 10)}.{f.Random.Number(0, 10)}")
           .RuleFor(t => t.Type, f => f.PickRandom<SensorType>())
           .RuleFor(x => x.FileName, f => f.System.FileName("txt"))
           .RuleFor(x => x.DeviceId, f => f.Random.Int(1000, 9999).ToString());

        var result = new List<Sensor>();
        result.AddRange(faker.Generate(100));

        return result;
    }
}

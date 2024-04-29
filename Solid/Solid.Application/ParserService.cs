using Newtonsoft.Json;
using Solid.Application.Interfaces;

namespace Solid.Application
{
    public class ParserService: IParser
    {
        public T Parse<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}

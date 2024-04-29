namespace Solid.Application.Interfaces;

public interface IParser
{
    T Parse<T>(string message);
}

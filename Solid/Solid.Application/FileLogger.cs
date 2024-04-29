using Solid.Application.Interfaces;

namespace Solid.Application
{
    public class FileLogger : IFileLogger
    {
        public void Write(string message)
        {
            // TOOD: Should we use serilog or another because now Ilogger we use like console.log in js
        }
    }

}

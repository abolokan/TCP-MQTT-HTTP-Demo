using System.Net;
using System.Runtime.Serialization;

namespace Solid.Common.Exceptions
{
    [Serializable]
    public class NoUpdateException : Exception
    {
        public NoUpdateException(string message) : base(message)
        {
        }

        protected NoUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}

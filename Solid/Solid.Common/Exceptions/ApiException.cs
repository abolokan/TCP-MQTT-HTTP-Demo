using System.Net;
using System.Runtime.Serialization;

namespace Solid.Common.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException(int httpStatusCode)
        {
            StatusCode = (HttpStatusCode)httpStatusCode;
        }

        public ApiException(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public ApiException(string message, int httpStatusCode) : base(message)
        {
            StatusCode = (HttpStatusCode)httpStatusCode;
        }

        public ApiException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public ApiException(string message, int httpStatusCode, Exception inner) : base(message, inner)
        {
            StatusCode = (HttpStatusCode)httpStatusCode;
        }

        public ApiException(string message, HttpStatusCode httpStatusCode, Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }

        protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public HttpStatusCode StatusCode { get; }
    }
}

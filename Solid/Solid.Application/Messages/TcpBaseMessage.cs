namespace Solid.Application.Messages
{
    public class TcpBaseMessage
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }

    public class TcpBaseMessage<T>
    {
        public string Action { get; set; }
        public T Data { get; set; }
    }
}

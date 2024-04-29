using Newtonsoft.Json;
using Solid.Application.Messages;

namespace Solid.Application.Tests
{
    public class ParserTests
    {
        private class TcpRequest<T>
        {
            public Guid Id { get; set; }
            public string Action { get; set; }
            public T Data { get; set; }
        }

        private class OnlineMessage
        {
            public string Test { get; set; }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var request = new TcpRequest<OnlineMessage>
            {
                Id = Guid.NewGuid(),
                Action = "Online",
                Data = new OnlineMessage
                {
                    Test = "Text",
                }
            };

            var objStr = JsonConvert.SerializeObject(request);

            var parser = new ParserService();


            var tcpMessage = parser.Parse<TcpBaseMessage>(objStr);

            var onlineMessage = parser.Parse<OnlineMessage>(tcpMessage.Data.ToString());
            Assert.AreEqual("Text", onlineMessage.Test);
        }
    }
}
using System;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TCPClientApp
{
    public partial class Form1 : Form
    {
        public const string TestDeviceId = "3613";
        private TcpClient _tcpClient;
        private NetworkStream networkStream;
        private StreamWriter streamWriter;
        public Form1()
        {
            InitializeComponent();
            RunTcpClient().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task RunTcpClient()
        {
            if (_tcpClient == null)
            {
                _tcpClient = new TcpClient();
            }

            if (_tcpClient.Connected)
            {
                return;
            }

            await _tcpClient.ConnectAsync("127.0.0.1", 5555);

            networkStream = _tcpClient.GetStream();
            streamWriter = new StreamWriter(networkStream);
        }

        public async Task SendMessage(string message)
        {
            if (_tcpClient.Connected)
            {
                await streamWriter.WriteAsync(message);
                await streamWriter.FlushAsync();
            }
            else
            {
                MessageBox.Show("Client disconnected");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RunTcpClient().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var data = new TcpBaseMessage<TcpDataMessage>
            {
                Action = "data",
                Data = new TcpDataMessage
                {
                    DeviceId = TestDeviceId,
                    Temp = 12.2,
                    TempC = 14.2
                }
            };

            SendMessage(JsonConvert.SerializeObject(data)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void buttonChangeDeviceId_Click(object sender, EventArgs e)
        {
            var data = new TcpBaseMessage<string>
            {
                Action = "device_id",
                Data = Guid.NewGuid().ToString().ToLower()
            };

            SendMessage(JsonConvert.SerializeObject(data)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            var action = new TcpBaseMessage<TcpDataMessage>
            {
                Action = "data",
                Data = new TcpDataMessage
                {
                    DeviceId = TestDeviceId,
                    Temp = 100,
                    TempC = 200
                }
            };

            SendMessage(JsonConvert.SerializeObject(action)).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        /// <summary>
        /// raw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSendRaw_Click(object sender, EventArgs e)
        {
            var action = new TcpBaseMessage<object>
            {
                Action = "raw",
                Data = new { anyData = "test", no = 1 }
            };

            SendMessage(JsonConvert.SerializeObject(action)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void buttonOnline_Click(object sender, EventArgs e)
        {
            var request = new TcpBaseMessage<TcpOnlineMessage>
            {
                Action = "online",
                Data = new TcpOnlineMessage
                {
                    Test = textBox1.Text,
                }
            };

            var objStr = JsonConvert.SerializeObject(request);
            SendMessage(objStr).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonError_Click(object sender, EventArgs e)
        {
            var action = new TcpBaseMessage<object>
            {
                Action = "error",
                Data = new { message = "error message" }
            };

            SendMessage(JsonConvert.SerializeObject(action)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            var action = new TcpBaseMessage<string>
            {
                Action = "disconnected",
                Data = string.Empty
            };

            SendMessage(JsonConvert.SerializeObject(action)).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }


    public class TcpBaseMessage<T>
    {
        public string Action { get; set; }
        public T Data { get; set; }
    }

    public class TcpOnlineMessage
    {
        public string Test { get; set; }
    }

    public class TcpDataMessage
    {
        public double? Temp { get; set; }
        public double? TempC { get; set; }
        public string DeviceId { get; set; }
        public TcpDataMessage Sub { get; set; }
    }

    public class TcpChangeIdMessage
    {
        public string OldId { get; set; }
        public string NewId { get; set; }
    }
}

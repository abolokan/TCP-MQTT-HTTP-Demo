namespace MqttClientApp;

public partial class Form1 : Form
{
    private readonly MqttClientWrapper _mqttClient;

    public Form1()
    {
        InitializeComponent();
        this.StartPosition = FormStartPosition.CenterScreen;

        _mqttClient = new MqttClientWrapper([
            "sensor/+/command",
            "sensor/+/data",
            "sensor/+/raw",
            "sensor/+/online",
            "sensor/+/offline",
            "sensor/+/error",
            "sensor/+/subdata",
        ]);
        _mqttClient.OnConnected += OnConnected;
        _mqttClient.OnReceivedMessage += AddHystory;
    }

    private void buttonConnect_Click_1(object sender, EventArgs e)
    {
        Task.Run(_mqttClient.ConnectAsync);

        AppendrichTextBoxSubcribtionsText("Connected!");
        buttonConnect.Enabled = false;
        richTextBoxMessage.Enabled = true;
    }

    private void buttonDisconnect_Click_1(object sender, EventArgs e)
    {
        Task.Run(_mqttClient.DisconnectAsync);

        buttonConnect.Enabled = true;
        richTextBoxMessage.Enabled = false;
        AppendrichTextBoxSubcribtionsText("Disconnected!");
    }

    private void buttonSendMessage_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(richTextBoxMessage.Text))
        {
            _mqttClient.PublishMessageAsync("sensor/w/command", richTextBoxMessage.Text).GetAwaiter().GetResult();
            richTextBoxMessage.Clear();
        }
        else
        {
            MessageBox.Show("Please type message");
        }
    }

    private void buttonClearHistory_Click(object sender, EventArgs e)
    {
        richTextBoxSubcribtions.Clear();
    }

    public void AddHystory(object sender, string message)
    {
        AppendrichTextBoxSubcribtionsText(message);
    }
    public void OnConnected(object sender, EventArgs e)
    {
    }

    private void AppendrichTextBoxSubcribtionsText(string message)
    {
        if (richTextBoxSubcribtions.InvokeRequired)
        {
            richTextBoxSubcribtions.Invoke((MethodInvoker)delegate
            {
                richTextBoxSubcribtions.AppendText(message + "\n");
            });
        }
        else
        {
            richTextBoxSubcribtions.AppendText(message + "\n");
        }
    }
}

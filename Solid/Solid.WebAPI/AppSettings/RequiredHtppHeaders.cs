namespace Solid.WebAPI.AppSettings;

public class RequiredHtppHeaders
{
    public const string Section = "RequiredHtppHeaders";

    public string[] ESP8266 { get; set; }
    public string[] ESP32 { get; set; }
}

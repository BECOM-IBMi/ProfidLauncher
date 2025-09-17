namespace ProfidLauncher.Models;

public class ProfidLauncherSettings
{
    public int HttpPort { get; set; }

    public int HttpsPort { get; set; }

    public string AppName { get; set; } = "";

    public string Prod { get; set; } = "";

    public string Test { get; set; } = "";

    public string China { get; set; } = "";
}

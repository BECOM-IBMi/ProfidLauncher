namespace ProfidLauncher.Models;

public enum Sites
{
    UNKNOWN,
    HOCHSTRASS,
    VIENNA,
    KOERNYE,
    HEYUAN
}

public class ProfidAppConfiguration
{
    public int Port { get; set; }

    public string OperationMode { get; set; } = "";

    public string Hostname { get; set; } = "";

    public bool IsNewWorkstation { get; set; }

    public string WorkstationId { get; set; } = "";

    public bool UseHTTPS { get; set; }

    public Sites Site { get; set; }

    public bool FromSelfUpdater { get; set; }
}

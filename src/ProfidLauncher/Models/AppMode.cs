namespace ProfidLauncher.Models;

public class AppMode
{
    public string OperationMode { get; set; } = "";

    public string IconName { get; set; } = "";

    public string BaseUrl { get; set; } = "";

    public string ProgramShortcutName { get; set; } = "";

    public bool UseHttps { get; set; }
}

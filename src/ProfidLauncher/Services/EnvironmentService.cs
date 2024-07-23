using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ProfidLauncher.Services;

public class EnvironmentService
{
    private readonly ILogger<EnvironmentService> _logger;

    private string _profidLauncherFolder = "";
    private string _currentProfile = "";

    public EnvironmentService(ILogger<EnvironmentService> logger)
    {
        _logger = logger;

        Initialize();
    }

    public static string GetProfidLauncherFolder()
    {
        var userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userProfileFolder, ".profid-launcher");
    }

    public void Initialize()
    {
        _profidLauncherFolder = GetProfidLauncherFolder();

        checkOrCreateEnvironment();
    }

    public string GetWebViewProfileFolder()
    {
        _logger.LogInformation($"Try to get the web view profile folder(s) (starts with _v)...");
        var pFolders = Directory.GetDirectories(_profidLauncherFolder, "_v*");
        _logger.LogInformation($"Found {pFolders.Length} folders!");

        string wvPRofileFolder;
        if (pFolders.Length > 0)
        {
            wvPRofileFolder = $"_v{pFolders.Length + 1}";
            _logger.LogInformation($"As there are already profile folders, create a new one with path {wvPRofileFolder}...");
        }
        else
        {
            wvPRofileFolder = "_v1";
            _logger.LogInformation($"There is no profile folders, create one with path {wvPRofileFolder}...");
        }

        var folder = Path.Combine(_profidLauncherFolder, wvPRofileFolder);
        Directory.CreateDirectory(folder);

        _currentProfile = folder;
        return folder;
    }

    public void Cleanup()
    {
        _logger.LogInformation($"Cleanup web view profile folder: {_currentProfile}...");
        if (Directory.Exists(_currentProfile))
        {
            Directory.Delete(_currentProfile, true);
        }
    }

    private void checkOrCreateEnvironment()
    {
        try
        {
            _logger.LogInformation($"Checking if Profid Launcher folder {_profidLauncherFolder} exists...");
            if (!Directory.Exists(_profidLauncherFolder))
            {
                _logger.LogInformation($"Folder not existing. Creating folder...");
                Directory.CreateDirectory(_profidLauncherFolder);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when creating environment: {ex.Message}!", ex);
        }
    }
}

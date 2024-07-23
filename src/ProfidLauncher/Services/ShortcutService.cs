using Microsoft.Extensions.Logging;
using ProfidLauncher.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ProfidLauncher.Services;


public class ShortcutService
{
    private readonly ILogger<ShortcutService> _logger;
    private readonly List<AppMode> _appModes;
    private readonly ProfidAppConfiguration _appConfig;

    public ShortcutService(ILogger<ShortcutService> logger, List<AppMode> appModes, ProfidAppConfiguration appConfig)
    {
        _logger = logger;
        _appModes = appModes;
        _appConfig = appConfig;
    }

    public void CreateShortcut(string operationMode)
    {
        // .\Shortcut.ps1 -target "C:\Users\mprattinge\source\repos\ProfidLauncher\V2\src\ProfidLauncher\bin\Debug\net7.0-windows\ProfidLauncher.exe" -targetArgs "-m ATRIUMP" -linkName "Profid Launcher.lnk" -icon "C:\Users\mprattinge\source\repos\ProfidLauncher\V2\src\ProfidLauncher\Assets\favicon_dev.ico"

        _logger.LogInformation($"Creating shortcut for operation mode {operationMode}");

        var mode = _appModes.First(x => x.OperationMode == operationMode);
        if (mode == null)
        {
            return;
        }

        //Struktur
        /*
         UpdatePath
            - ProfidLauncherUpdater.exe
            - appsettings.json -> Appsettings for Updater
            - Assets           -> Folder with icons to use when a shortcut is created
            - info.json        -> Versioninfo for ProfidLauncher (Which version to start)
            - vX.Y.Z           -> Folder with current Version ProfidLauncher init
            - vA.B.C           -> Folder with prev. Version of ProfidLauncher
            - tmp              -> Folder where versions are downloaded

        AppContext.BaseDirectory -> vX.Y.Z Direcotry
        From our current directory we need to find the Asset directory in the UpdateFolder
         */
        var baseDir = AppContext.BaseDirectory;
        var baseDirInfo = new DirectoryInfo(baseDir);
        if (baseDirInfo is null || !baseDirInfo.Exists)
        {
            throw new ArgumentException("Basedirectory is null");
        }

        var updateDirInfo = baseDirInfo.Parent;
        if (updateDirInfo is null || !updateDirInfo.Exists)
        {
            throw new ArgumentException("Updatedirectory is null");
        }


        var assetsDir = Path.Combine(baseDir, "Assets");

        var script = Path.Combine(assetsDir, "Shortcut.ps1");

        var app = $"ProfidLauncherUpdater.exe";
        var appPath = Path.Combine(updateDirInfo.FullName, app);
        var arg = $"run {operationMode}";

        var iconAssetPath = Path.Combine(updateDirInfo.FullName, "Assets");
        var iconPath = Path.Combine(iconAssetPath, mode.IconName);

        var lnkName = $"{mode.ProgramShortcutName}.lnk";

        var scriptArgs = $"-target \"{appPath}\" -targetArgs \"{arg}\" -linkName \"{lnkName}\" -icon \"{iconPath}\" -workDir \"{updateDirInfo.FullName}\"";

        var scriptArguments = $"-ExecutionPolicy Bypass -File \"{script}\" {scriptArgs}";

        _logger.LogInformation($"Shortcut Powershell script ({script} will be called with this args: ({scriptArgs})");

        try
        {
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(output))
            {
                _logger.LogInformation($"Creating-Shortcut response: {output}");
            }

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating shortcut with powershell: {ex.Message}", ex);
        }
    }
}
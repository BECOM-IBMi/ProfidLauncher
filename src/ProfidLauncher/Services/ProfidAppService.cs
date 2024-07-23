using Microsoft.Extensions.Logging;
using ProfidLauncher.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProfidLauncher.Services;

public class ProfidAppService
{
    private readonly ILogger<ProfidAppService> _logger;
    private readonly List<AppMode> _appModes;
    private readonly ProfidAppConfiguration _profidConfig;
    private readonly ProfidLauncherSettings _settings;
    private readonly AppMode _appMode;

    private string _version = "";

    public ProfidAppService(ILogger<ProfidAppService> logger, List<AppMode> appModes, ProfidAppConfiguration profidConfig, ProfidLauncherSettings settings)
    {
        _logger = logger;
        _appModes = appModes;
        _profidConfig = profidConfig;
        _settings = settings;

        //Richtiges Customizing finden
        _logger.LogDebug($"Lookup correct app config...");
        var appMode = _appModes.FirstOrDefault(x => x.OperationMode == _profidConfig.OperationMode);
        if (appMode is null)
        {
            var msg = $"Couldn't find app settings for operation mode {_profidConfig.OperationMode}";
            _logger.LogError(msg);
            throw new Exception(msg);
        }

        _appMode = appMode;
    }

    public AppMode GetCurrentAppMode()
    {
        return _appMode;
    }

    public string GetCurrentUrl()
    {


        _logger.LogDebug($"Preparing url...");
        //Url aufbereiten
        var url = _appMode.BaseUrl;

        var port = 0;
        var protocol = "";
        var useAuth = "";
        if (_profidConfig.Port != 0)
        {
            //Teile der Url aus den args Überschreiben
            _logger.LogInformation($"Port overwritten via commandline argument: {_profidConfig.Port}");

            port = _profidConfig.Port;
            protocol = _profidConfig.UseHTTPS ? "https" : "http";
            useAuth = _profidConfig.UseHTTPS ? "auth/" : "";
        }
        else
        {
            //Url aus customizing aufbereiten
            port = _appMode.UseHttps ? _settings.HttpsPort : _settings.HttpPort;
            protocol = _appMode.UseHttps ? "https" : "http";
            useAuth = _appMode.UseHttps ? "auth/" : "";
        }

        url = string.Format(url, protocol, port, useAuth, _profidConfig.WorkstationId.ToLower());

        return url;
    }

    public string GetTitle()
    {
        //Der Titel baut sich folgendermaßen auf:
        // {AppNAme} - {Version} - {Url}

        return $"{_settings.AppName} - {Version} - {GetCurrentUrl()}";

    }

    public ImageSource GetWindowIcon()
    {
        var iconName = _appMode.IconName;

        //var applicationFilePath = Assembly.GetExecutingAssembly().Location;
        var applicationFilePath = AppDomain.CurrentDomain.BaseDirectory;
        _logger.LogInformation($"Detected fullpath of current assembly is: {applicationFilePath}!, Extracting folder...");

        var appDir = Path.GetDirectoryName(applicationFilePath);
        if (appDir == null)
        {
            var msg = "Cannot find Application folder!";
            throw new Exception(msg);

        }
        string iconPath = Path.Combine(appDir, "Assets", iconName);

        _logger.LogInformation($"Checking icon path {iconPath}...");
        if (iconPath == null || !File.Exists(iconPath))
        {
            var msg = "Cannot find application icon!";
            _logger.LogError(msg);
            throw new Exception(msg);
        }

        _logger.LogInformation($"Setting window icon...");

        var ico = new Icon(iconPath);

        var source = Imaging.CreateBitmapSourceFromHIcon(
            ico.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

        return source;
    }

    public string Version
    {
        get
        {
            if (string.IsNullOrEmpty(_version))
            {
                var version = "1.0.0+LOCALBUILD";
                var appAssembly = typeof(ProfidAppService).Assembly;

                if (appAssembly != null)
                {
                    var attrs = appAssembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));
                    if (attrs != null)
                    {
                        var infoVerAttr = (AssemblyInformationalVersionAttribute)attrs;
                        if (infoVerAttr != null && infoVerAttr.InformationalVersion.Length > 6)
                        {
                            version = infoVerAttr.InformationalVersion;
                        }
                    }
                }

                if (version.Contains('+'))
                {
                    version = version[..version.IndexOf('+')];
                }

                if (version.Contains("-"))
                {
                    var sub = version[version.IndexOf('-')..];
                    sub = sub[(sub.IndexOf('.') + 1)..];

                    version = version[..version.IndexOf('-')];
                    version += "." + sub + "-dev";
                    _version = version;
                }
                else
                {
                    _version = version;
                }
            }

            _logger.LogDebug($"Current version of the app is {_version}");

            return _version;
        }
    }
}

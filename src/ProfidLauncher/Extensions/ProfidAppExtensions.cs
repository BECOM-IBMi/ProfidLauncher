
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ProfidLauncher.Models;
using Serilog;
using System;
using System.Linq;
using System.Text.Json;

namespace ProfidLauncher.Extensions;

public static class ProfidAppExtensions
{
    public static IServiceCollection AddProfidAppSettings(this IServiceCollection services, string[] args)
    {
        Log.Information("Parsing commandline args...");
        var res = Parser.Default.ParseArguments<CommandLineOptions>(args);

        Log.Information("Detecting hostname...");
        var hostname = System.Net.Dns.GetHostName();
        Log.Information($"Hostname is {hostname}! Extracting workstation information and configuration...");

        var appConfig = GetProfidAppConfiguration(res.Value, hostname);

        Log.Information($"Following app configuration detected: {JsonSerializer.Serialize(appConfig)}");

        services.AddSingleton(appConfig);

        return services;
    }

    public static ProfidAppConfiguration GetProfidAppConfiguration(CommandLineOptions opts, string hostname)
    {
        try
        {
            var workstationInfo = GetWorkstationInfo(hostname);

            var appConfig = new ProfidAppConfiguration();
            appConfig.Port = opts.Port;
            appConfig.OperationMode = opts.OperationMode;
            appConfig.IsNewWorkstation = workstationInfo.isNewWorkstation;
            appConfig.WorkstationId = workstationInfo.workstationId;
            appConfig.Site = workstationInfo.site;
            appConfig.UseHTTPS = opts.UseSecure;

            return appConfig;
        }
        catch (System.Exception ex)
        {
            var msg = $"Error when getting app configuration: {ex.Message}";
            Log.Error(msg, ex);
            throw new Exception(msg, ex);
        }
    }

    public static (bool isNewWorkstation, string workstationId, Sites site) GetWorkstationInfo(string hostname)
    {
        bool isNew = false;
        string wId = "";

        if (hostname.Contains("-"))
        {
            wId = GetWorkstationIdNew(hostname);
            isNew = true;
        }
        else
        {
            wId = getWorkstationIdOld(hostname);
            isNew = false;
        }

        var site = Sites.UNKNOWN;

        if (isNew)
        {
            var mandant = hostname.Substring(0, 3);
            if (int.TryParse(mandant, out int m))
            {
                site = m switch
                {
                    1 => Sites.HOCHSTRASS,
                    2 => Sites.KOERNYE,
                    3 => Sites.VIENNA,
                    32 => Sites.HEYUAN,
                    _ => Sites.UNKNOWN
                };
            }
        }

        return (isNew, wId, site);
    }

    private static string GetWorkstationIdNew(string hostname)
    {
        var parts = hostname.Split('-');
        var id = $"{parts.Last()}";
        id = trimLeft(id, 7);
        return id;
    }

    public static string getWorkstationIdOld(string hostname)
    {
        var id = hostname.Substring(3, hostname.Length - 3);
        return id;
    }

    private static string trimLeft(string text, int length)
    {
        if (text.Length <= length) return text;
        var cutoff = text.Length - length;
        return text.Substring(cutoff, length);
    }
}

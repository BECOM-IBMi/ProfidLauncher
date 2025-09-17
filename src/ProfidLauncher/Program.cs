using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfidLauncher.Extensions;
using ProfidLauncher.Services;
using ProfidLauncher.ViewModels;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProfidLauncher;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var baseFolder = EnvironmentService.GetProfidLauncherFolder();
        var logFile = Path.Combine(baseFolder, "logs", "ProfidLog.txt");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
            .WriteTo.ToMemory()
            .CreateLogger();

        Log.Information("Dir: " + Directory.GetCurrentDirectory());
        Log.Information("Dir: " + AppContext.BaseDirectory);

        var host = Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((ctx, services) =>
            {
                services.AddLogging(loggingBuilder =>
                    loggingBuilder.AddSerilog(dispose: true));

                services.AddProfidCustomizing(ctx.Configuration);
                services.AddProfidAppSettings(args);

                services.AddSingleton<ProfidAppService>();
                services.AddSingleton<EnvironmentService>();
                services.AddSingleton<ProfidLauncherCommunicator>();
                services.AddSingleton<ShortcutService>();

                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<Admin>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<AdminViewModel>();
            })
            .Build();

        var app = host.Services.GetService<App>();
        if (app is null)
        {
            Log.Logger.Error("Couln't allocate app service");
            return;
        }
        app.Run();


        var envService = host.Services.GetService<EnvironmentService>();

        Task.Delay(2000).Wait();
        Log.Logger.Information($"Form is closed! Cleanup environment...");
        envService!.Cleanup();

        Log.Logger.Information("Profid Launcher ended!");
    }
}
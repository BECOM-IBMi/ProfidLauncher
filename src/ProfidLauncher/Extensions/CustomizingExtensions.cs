using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfidLauncher.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfidLauncher.Extensions;

public static class CustomizingExtensions
{
    public static IServiceCollection AddProfidCustomizing(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Loading profid customizing from appsettings...");
        var profidConfig = new ProfidLauncherSettings();
        configuration.GetSection("ProfidLauncherSettings").Bind(profidConfig);
        services.AddSingleton(profidConfig);

        //Customizing aus den appsettings laden
        Log.Information("Loading app customizing from appsettings...");
        var appModes = new List<AppMode>();
        configuration.GetSection("AppMode").Bind(appModes);

        services.AddSingleton(appModes);

        return services;
    }
}

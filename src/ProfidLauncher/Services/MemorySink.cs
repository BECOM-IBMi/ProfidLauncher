using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace ProfidLauncher.Services;

public class MemorySink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        UILogService.AddEntry(logEvent.Timestamp, logEvent.MessageTemplate.Text);
    }
}

public static class MemorySinkExtensions
{
    const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    public static LoggerConfiguration ToMemory(this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            LoggingLevelSwitch? levelSwitch = null)
    {
        if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
        if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

        return sinkConfiguration.Sink(new MemorySink(), restrictedToMinimumLevel, levelSwitch);
    }
}

public class UILogService
{
    public static Dictionary<DateTimeOffset, string> Logs = new Dictionary<DateTimeOffset, string>();

    public static void AddEntry(DateTimeOffset time, string log)
    {
        Logs.Add(time, log);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ProfidLauncher.Services;
using System;
using System.Collections.Generic;

namespace ProfidLauncher.ViewModels;

public partial class AdminViewModel : ObservableObject
{
    private readonly ILogger<AdminViewModel> _logger;
    private readonly ShortcutService _shortcutService;

    [ObservableProperty]
    private Dictionary<DateTimeOffset, string> logs = new();

    public AdminViewModel(ILogger<AdminViewModel> logger, ShortcutService shortcutService)
    {
        _logger = logger;
        _shortcutService = shortcutService;
    }

    [RelayCommand]
    private void PageLoaded()
    {
        Logs = new Dictionary<DateTimeOffset, string>(UILogService.Logs);
    }

    [RelayCommand]
    private void CreateGenieIcon()
    {
        try
        {
            _shortcutService.CreateShortcut("GENIEP");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Genie Icon: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CreateAtriumDevIcon()
    {
        try
        {
            _shortcutService.CreateShortcut("ATRIUMD");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Atrium Dev Icon: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CreateGenieDevIcon()
    {
        try
        {

            _shortcutService.CreateShortcut("GENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Genie Dev Icon: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CreateAtriumCNIcon()
    {
        try
        {
            _shortcutService.CreateShortcut("ATRIUMCN");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Atrium China Icon: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CreateGenieCNIcon()
    {
        try
        {
            _shortcutService.CreateShortcut("GENIECN");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Genie China Icon: {ex.Message}");
        }
    }
}

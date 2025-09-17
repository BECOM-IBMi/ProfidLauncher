using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ProfidLauncher.Services;
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace ProfidLauncher.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly ProfidAppService _profidService;
    private readonly Admin _adminPage;
    private readonly ShortcutService _shortcutService;

    [ObservableProperty]
    private string profidUrl = "";

    [ObservableProperty]
    private string appTitle = "";

    [ObservableProperty]
    private ImageSource windowIcon = default!;

    public MainViewModel(ILogger<MainViewModel> logger, ProfidAppService profidService, Admin adminPage, ShortcutService shortcutService)
    {
        _logger = logger;
        _profidService = profidService;
        _adminPage = adminPage;
        _shortcutService = shortcutService;
    }

    [RelayCommand]
    private void PageLoaded()
    {
        try
        {
            //Url ermitteln und Source setzten
            var url = _profidService.GetCurrentUrl();
            ProfidUrl = url;

            var title = _profidService.GetTitle();
            AppTitle = title;

            WindowIcon = _profidService.GetWindowIcon();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when preparing the application: {ex.Message}", ex.Message);
        }
    }

    [RelayCommand]
    private void ShowHelp()
    {
        ProcessStartInfo sInfo = new ProcessStartInfo("https://becom-group.atlassian.net/wiki/spaces/ITD/pages/90115211/DE+-+PROFID+Launcher")
        {
            UseShellExecute = true
        };
        Process.Start(sInfo);
    }

    [RelayCommand]
    private void ShowInfo()
    {
        var info = new Info();
        info.ShowDialog();
    }

    [RelayCommand]
    private void ShowAdmin()
    {
        _adminPage.ShowDialog();
    }

    [RelayCommand]
    private void AddChinaShortcut()
    {
        try
        {
            _shortcutService.CreateShortcut("ATRIUMCN");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when creating profid launcher china shorctut: {ErrorMessage}", ex.Message);
        }
    }
}
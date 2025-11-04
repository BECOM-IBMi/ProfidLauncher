using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using ProfidLauncher.Models;
using ProfidLauncher.Services;
using ProfidLauncher.ViewModels;
using System;
using System.Text.Json;
using System.Windows;

namespace ProfidLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;
        private readonly EnvironmentService _envService;
        private readonly ProfidLauncherCommunicator _profidLauncherCommuncator;

        public MainWindow(ILogger<MainWindow> logger, MainViewModel viewModel, EnvironmentService envService, ProfidLauncherCommunicator profidLauncherCommuncator)
        {
            _logger = logger;
            _envService = envService;
            _profidLauncherCommuncator = profidLauncherCommuncator;

            InitializeComponent();

            this.DataContext = viewModel;
        }

        protected override async void OnSourceInitialized(EventArgs e)
        {
            try
            {
                var env = CoreWebView2Environment.CreateAsync(null, _envService.GetWebViewProfileFolder()).Result;
                await webView.EnsureCoreWebView2Async(env);

                webView.CoreWebView2.AddHostObjectToScript("plauncher", _profidLauncherCommuncator);

                // This force disable the Ctrl+R refresh and F5 refresh
                webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when creating profid web view environment: {ex.Message}!");
                return;
            }


        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            await Dispatcher.InvokeAsync(async () =>
            {
                string res = await webView.CoreWebView2.ExecuteScriptAsync("window.BECOM.atriumHelper.hasTabsOpen()");
                if (string.IsNullOrEmpty(res) || res == "null")
                {
                    Application.Current.Shutdown();
                    return;
                }

                var openTabsResult = JsonSerializer.Deserialize<OpenTabsRequestResult>(res);
                if (openTabsResult == null || openTabsResult.HasOpenTabs == false)
                {
                    Application.Current.Shutdown();
                    return;
                }

                //Es gibt noch aktive Tabs, schließen nicht möglich
                MessageBox.Show("You have open Tabs in Profid, please close all tabs first!", "Open Tabs", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }
    }
}
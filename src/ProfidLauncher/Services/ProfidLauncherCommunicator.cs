using System.Runtime.InteropServices;
using System.Windows;

namespace ProfidLauncher.Services
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ProfidLauncherCommunicator
    {
        public string Name { get; set; } = "Profid Launcher";

        public void LoggedOff()
        {
            Application.Current.Shutdown();
        }
    }
}

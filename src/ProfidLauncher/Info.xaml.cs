using System.Windows;

namespace ProfidLauncher
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info() : this("1.0.0")
        {
        }

        public Info(string version)
        {
            InitializeComponent();
            InfoBox.Version = version;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using ProfidLauncher.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace ProfidLauncher
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin(AdminViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
            return;
        }
    }
}

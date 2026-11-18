using System.Windows.Controls;
using TimeTracker.Desktop.ViewModels;

namespace TimeTracker.Desktop.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
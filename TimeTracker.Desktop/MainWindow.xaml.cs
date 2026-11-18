using TimeTracker.Desktop.Tracking;
using TimeTracker.Desktop.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TimeTracker.Desktop
{
    public partial class MainWindow : FluentWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            SystemThemeWatcher.Watch(this);

            Loaded += (_, _) => RootNavigation.Navigate(typeof(TodayPage));

            App.Tracker.Start();
        }
    }
}
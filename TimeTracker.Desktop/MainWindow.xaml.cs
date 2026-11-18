using TimeTracker.Desktop.Tracking;
using TimeTracker.Desktop.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TimeTracker.Desktop
{
    public partial class MainWindow : FluentWindow
    {
        private readonly ActivityTracker _tracker = new ActivityTracker();

        public MainWindow()
        {
            InitializeComponent();

            SystemThemeWatcher.Watch(this);

            Loaded += (_, _) => RootNavigation.Navigate(typeof(TodayPage));

            _tracker.Start();
        }
    }
}
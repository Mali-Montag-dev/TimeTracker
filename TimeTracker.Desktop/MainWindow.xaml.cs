using System.ComponentModel;
using System.Windows;
using TimeTracker.Desktop.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TimeTracker.Desktop
{
    public partial class MainWindow : FluentWindow
    {
        private bool _isExiting;

        public MainWindow()
        {
            InitializeComponent();

            SystemThemeWatcher.Watch(this);

            Loaded += (_, _) => RootNavigation.Navigate(typeof(TodayPage));

            App.Tracker.Start();
        }

        // לחיצה על ✕: מסתירים במקום לסגור
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_isExiting)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            base.OnClosing(e);
        }

        private void ShowFromTray()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void TrayIcon_LeftClick(object sender, RoutedEventArgs e) => ShowFromTray();

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e) => ShowFromTray();

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _isExiting = true;
            App.Tracker.Stop();
            Application.Current.Shutdown();
        }
    }
}
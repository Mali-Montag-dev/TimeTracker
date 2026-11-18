using System.Windows;
using TimeTracker.Desktop.Services;
using TimeTracker.Desktop.Tracking;

namespace TimeTracker.Desktop
{
    public partial class App : Application
    {
        public static ApiClient Api { get; } = new ApiClient();
        public static ActivityTracker Tracker { get; } = new ActivityTracker();

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();

            if (e.Args.Contains("--minimized"))
            {
                // מפעילים מוסתרים: רק האייקון ליד השעון
                window.WindowState = WindowState.Minimized;
                window.Show();
                window.Hide();
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.Show();
            }

            await ApiLauncher.EnsureRunningAsync();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ApiLauncher.Stop();
            base.OnExit(e);
        }
    }
}
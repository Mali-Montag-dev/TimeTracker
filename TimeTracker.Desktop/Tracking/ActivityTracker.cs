using System.Net.Http;
using System.Windows.Threading;
using TimeTracker.Desktop.Services;
using System.Diagnostics;

namespace TimeTracker.Desktop.Tracking
{
    public class ActivityTracker
    {
        private static readonly TimeSpan IdleThreshold = TimeSpan.FromMinutes(5);
        private static readonly string OwnProcessName = Process.GetCurrentProcess().ProcessName;

        private readonly ApiClient _api = new ApiClient();
        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };

        public string CurrentApp { get; private set; } = string.Empty;
        public bool IsApiAvailable { get; private set; }

        public ActivityTracker()
        {
            _timer.Tick += Timer_Tick;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if (WindowsApi.GetIdleTime() > IdleThreshold)
                return;

            var report = WindowsApi.GetActiveWindow();
            if (report == null)
                return;

            // לא סופרים את הזמן שבו מסתכלים על האפליקציה עצמה
            if (string.Equals(report.ProcessName, OwnProcessName, StringComparison.OrdinalIgnoreCase))
                return;

            CurrentApp = report.DisplayName;

            try
            {
                await _api.ReportActivityAsync(report);
                IsApiAvailable = true;
            }
            catch (HttpRequestException)
            {
                IsApiAvailable = false;
            }
        }
    }
}
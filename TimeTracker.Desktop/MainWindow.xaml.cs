using System.Net.Http;
using System.Windows;
using System.Windows.Threading;
using TimeTracker.Desktop.Services;
using TimeTracker.Desktop.Tracking;

namespace TimeTracker.Desktop
{
    public partial class MainWindow : Window
    {
        private static readonly TimeSpan IdleThreshold = TimeSpan.FromMinutes(5);

        private readonly ApiClient _api = new ApiClient();
        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };

        public MainWindow()
        {
            InitializeComponent();
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            // אם אין פעילות כבר 5 דקות, לא מדווחים
            if (WindowsApi.GetIdleTime() > IdleThreshold)
            {
                StatusText.Text = "אין פעילות, הזמן לא נספר";
                return;
            }

            var report = WindowsApi.GetActiveWindow();
            if (report == null)
                return;

            CurrentAppText.Text = report.DisplayName;

            try
            {
                await _api.ReportActivityAsync(report);
                StatusText.Text = $"דווח בשעה {DateTime.Now:HH:mm:ss}";
            }
            catch (HttpRequestException)
            {
                StatusText.Text = "⚠️ ה-API לא זמין";
            }
        }
    }
}
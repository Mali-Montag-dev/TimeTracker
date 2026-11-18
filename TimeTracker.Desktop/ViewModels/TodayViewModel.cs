using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TimeTracker.Desktop.Helpers;

namespace TimeTracker.Desktop.ViewModels
{
    public partial class TodayViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30)
        };

        [ObservableProperty] private string _dateText = string.Empty;
        [ObservableProperty] private string _totalTime = "–";
        [ObservableProperty] private string _productiveTime = "–";
        [ObservableProperty] private string _focusPercent = "–";
        [ObservableProperty] private string _currentApp = "–";
        [ObservableProperty] private bool _hasError;

        public ObservableCollection<AppRow> Apps { get; } = new ObservableCollection<AppRow>();

        public TodayViewModel()
        {
            var hebrew = new CultureInfo("he-IL");
            DateText = DateTime.Today.ToString("dddd, d בMMMM yyyy", hebrew);

            _timer.Tick += async (_, _) => await RefreshAsync();
        }

        public void Start()
        {
            _timer.Start();
            _ = RefreshAsync();
        }

        public void Stop() => _timer.Stop();

        [RelayCommand]
        private async Task RefreshAsync()
        {
            CurrentApp = string.IsNullOrEmpty(App.Tracker.CurrentApp) ? "–" : App.Tracker.CurrentApp;

            try
            {
                var report = await App.Api.GetDailyReportAsync(DateTime.Today);

                TotalTime = TimeFormatter.Format(report.TotalMinutes);
                ProductiveTime = TimeFormatter.Format(report.ProductiveMinutes);
                FocusPercent = report.TotalMinutes > 0
                    ? $"{report.ProductiveMinutes / report.TotalMinutes:P0}"
                    : "–";

                Apps.Clear();
                var max = report.Apps.Count > 0 ? report.Apps.Max(a => a.Minutes) : 1;

                foreach (var app in report.Apps.Where(a => a.Minutes >= 0.5))
                {
                    Apps.Add(new AppRow(
                        app.DisplayName,
                        app.CategoryName ?? "ללא קטגוריה",
                        TimeFormatter.Format(app.Minutes),
                        app.Minutes / max * 100));
                }

                HasError = false;
            }
            catch (HttpRequestException)
            {
                HasError = true;
            }
        }
    }

    public record AppRow(string Name, string Category, string Time, double Percent);
}
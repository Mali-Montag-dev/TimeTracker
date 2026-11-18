using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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

        // ← חדש: המקרא של התרשים
        public ObservableCollection<CategoryRow> Categories { get; } = new ObservableCollection<CategoryRow>();

        // ← חדש: הפרוסות של התרשים
        [ObservableProperty] private ISeries[] _categorySeries = Array.Empty<ISeries>();

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

                // ===== כרטיסי סיכום =====
                TotalTime = TimeFormatter.Format(report.TotalMinutes);
                ProductiveTime = TimeFormatter.Format(report.ProductiveMinutes);
                FocusPercent = report.TotalMinutes > 0
                    ? $"{report.ProductiveMinutes / report.TotalMinutes:P0}"
                    : "–";

                // ===== רשימת תוכנות =====
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

                // ===== ← חדש: תרשים קטגוריות =====
                var categories = report.Categories.Where(c => c.Minutes >= 0.5).ToList();

                CategorySeries = categories
                    .Select(c => (ISeries)new PieSeries<double>
                    {
                        Values = new[] { c.Minutes },
                        Name = c.Name,
                        Fill = new SolidColorPaint(SKColor.Parse(c.Color)),
                        InnerRadius = 70
                    })
                    .ToArray();

                // ===== ← חדש: מקרא קטגוריות =====
                Categories.Clear();
                foreach (var c in categories)
                {
                    Categories.Add(new CategoryRow(
                        c.Name,
                        TimeFormatter.Format(c.Minutes),
                        c.Color,
                        $"{c.Minutes / report.TotalMinutes:P0}"));
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

    // ← חדש
    public record CategoryRow(string Name, string Time, string Color, string Percent);
}
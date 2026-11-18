using System.Windows;
using TimeTracker.Desktop.Services;
using TimeTracker.Desktop.Tracking;

namespace TimeTracker.Desktop
{
    public partial class App : Application
    {
        public static ApiClient Api { get; } = new ApiClient();
        public static ActivityTracker Tracker { get; } = new ActivityTracker();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using TimeTracker.Desktop.Services;

namespace TimeTracker.Desktop.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly bool _ready;

        [ObservableProperty] private bool _runAtStartup;

        public string Version { get; } = "גרסה 1.1";

        public SettingsViewModel()
        {
            RunAtStartup = StartupManager.IsEnabled();
            _ready = true;
        }

        partial void OnRunAtStartupChanged(bool value)
        {
            if (_ready)
                StartupManager.SetEnabled(value);
        }
    }
}
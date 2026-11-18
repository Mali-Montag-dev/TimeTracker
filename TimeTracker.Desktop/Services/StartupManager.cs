using Microsoft.Win32;

namespace TimeTracker.Desktop.Services
{
    public static class StartupManager
    {
        private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "TimeTracker";

        public static bool IsEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKey);
            return key?.GetValue(AppName) != null;
        }

        public static void SetEnabled(bool enabled)
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: true)
                            ?? Registry.CurrentUser.CreateSubKey(RunKey);

            if (enabled)
                key.SetValue(AppName, $"\"{Environment.ProcessPath}\" --minimized");
            else
                key.DeleteValue(AppName, throwOnMissingValue: false);
        }
    }
}
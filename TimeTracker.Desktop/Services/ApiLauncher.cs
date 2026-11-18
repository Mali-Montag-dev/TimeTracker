using System.Diagnostics;
using System.IO;

namespace TimeTracker.Desktop.Services
{
    public static class ApiLauncher
    {
        private static Process? _apiProcess;

        public static async Task EnsureRunningAsync()
        {
            // אם השרת כבר רץ (למשל מ-Visual Studio), אין מה לעשות
            if (await App.Api.IsAliveAsync())
                return;

            var exePath = Path.Combine(AppContext.BaseDirectory, "Api", "TimeTracker.API.exe");
            if (!File.Exists(exePath))
                return;

            _apiProcess = Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = Path.GetDirectoryName(exePath)!,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            // מחכים עד 60 שניות שהשרת יעלה (בהפעלה הראשונה LocalDB לוקח זמן)
            for (int i = 0; i < 120; i++)
            {
                await Task.Delay(500);
                if (await App.Api.IsAliveAsync())
                    return;
            }
        }

        public static void Stop()
        {
            if (_apiProcess != null && !_apiProcess.HasExited)
            {
                _apiProcess.Kill();
                _apiProcess.Dispose();
            }
        }
    }
}
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TimeTracker.DTO;

namespace TimeTracker.Desktop.Tracking
{
    public static class WindowsApi
    {
        // ===== פונקציות של Windows =====

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO info);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        // ===== החלון הפעיל =====

        public static ActivityReportDto? GetActiveWindow()
        {
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
                return null;

            var title = new StringBuilder(512);
            GetWindowText(handle, title, title.Capacity);

            GetWindowThreadProcessId(handle, out uint processId);

            try
            {
                using var process = Process.GetProcessById((int)processId);

                string displayName = process.ProcessName;
                try
                {
                    var description = process.MainModule?.FileVersionInfo.FileDescription;
                    if (!string.IsNullOrWhiteSpace(description))
                        displayName = description;
                }
                catch
                {
                    // תוכנות מערכת מסוימות לא מרשות לקרוא את הפרטים שלהן. נשאר עם השם הטכני.
                }

                return new ActivityReportDto
                {
                    ProcessName = process.ProcessName,
                    DisplayName = displayName,
                    WindowTitle = title.ToString()
                };
            }
            catch (ArgumentException)
            {
                // התוכנה נסגרה בדיוק ברגע הבדיקה
                return null;
            }
        }

        // ===== זמן חוסר פעילות =====

        public static TimeSpan GetIdleTime()
        {
            var info = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>() };
            if (!GetLastInputInfo(ref info))
                return TimeSpan.Zero;

            uint idleMilliseconds = (uint)Environment.TickCount - info.dwTime;
            return TimeSpan.FromMilliseconds(idleMilliseconds);
        }
    }
}
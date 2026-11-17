namespace TimeTracker.DAL.Entities
{
    public class ActivitySession
    {
        public int Id { get; set; }
        public int TrackedAppId { get; set; }
        public TrackedApp TrackedApp { get; set; } = null!;   // ← ה-Navigation
        public string WindowTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
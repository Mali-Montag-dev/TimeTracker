

namespace TimeTracker.DTO
{
    public class TrackedAppDto
    {
        public int Id { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}

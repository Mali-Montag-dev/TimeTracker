

namespace TimeTracker.DAL.Entities
{
    public class TrackedApp
    {
        public int Id { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        


        public ICollection<ActivitySession> Sessions { get; set; } = new List<ActivitySession>();

    }
}

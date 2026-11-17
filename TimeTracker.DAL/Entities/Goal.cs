namespace TimeTracker.DAL.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int DailyMinutes { get; set; }
        public bool IsLimit { get; set; }
    }
}
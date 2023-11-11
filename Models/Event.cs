namespace Assignment1v3.Models
{
    public class Event
    {
        public int id { get; set; }
        public string? title { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime startRecur { get; set; }
        public DateTime endRecur { get; set; }
        public string? daysOfWeek { get; set; }
        public string? url { get; set; }
        public int courseId { get; set; }
        public string? userId { get; set; }
    }
}

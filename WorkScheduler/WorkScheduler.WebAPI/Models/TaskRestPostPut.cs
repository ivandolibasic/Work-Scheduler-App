namespace WorkScheduler.WebAPI.Models
{
    public class TaskRestPostPut
    {
        public string Description { get; set; }
        public int TotalHoursNeeded { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
    }
}
using System;

namespace WorkScheduler.WebAPI.Models
{
    public class TaskRestGet
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int TotalHoursNeeded { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }

        public TaskRestGet(Guid id, string description, int totalHoursNeeded, DateTime dateCreated, DateTime dateUpdated, string status, string username)
        {
            this.Id = id;
            this.Description = description;
            this.TotalHoursNeeded = totalHoursNeeded;
            this.DateCreated = dateCreated;
            this.DateUpdated = dateUpdated;
            this.Status = status;
            this.Username = username;
        }
    }
}
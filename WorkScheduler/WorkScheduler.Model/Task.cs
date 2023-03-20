using System;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class Task : ITask
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int TotalHoursNeeded { get; set; }
        public Guid TaskStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid UpdatedByUser { get; set; }
        public string Username { get; set; }
    }
}
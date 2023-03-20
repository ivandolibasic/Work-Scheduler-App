using System;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class TaskStatus : ITaskStatus
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        public TaskStatus()
        {

        }

        public TaskStatus(Guid id, string status)
        {
            this.Id = id;
            this.Status = status;
        }
    }
}
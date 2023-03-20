using System;

namespace WorkScheduler.WebAPI.Models
{
    public class TaskStatusRest
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        public TaskStatusRest()
        {

        }

        public TaskStatusRest(Guid id)
        {
            this.Id = id;
        }

        public TaskStatusRest(string status)
        {
            this.Status = status;
        }

        public TaskStatusRest(Guid id, string status)
        {
            this.Id = id;
            this.Status = status;
        }
    }
}
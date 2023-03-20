using System;

namespace WorkScheduler.WebAPI.Models
{
    public class TaskRest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int TotalHoursNeeded { get; set; }
        public Guid TaskStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid UpdatedByUser { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }

        public TaskRest()
        {

        }

        public TaskRest(string description, int totalHoursNeeded)
        {
            this.Id = Guid.NewGuid();
            this.Description = description;
            this.TotalHoursNeeded = totalHoursNeeded;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = this.DateCreated;
        }

        public TaskRest(Guid id, string description, int totalHoursNeeded, string status, string username)
        {
            this.Id = id;
            this.Description = description;
            this.TotalHoursNeeded = totalHoursNeeded;
            this.DateCreated = DateCreated;
            this.DateUpdated = DateTime.Now;
            this.Status = status;
            this.Username = username;
        }

        public WorkScheduler.Model.Task MapToTaskPost()
        {
            return new WorkScheduler.Model.Task()
            {
                Id = Guid.NewGuid(),
                Description = this.Description,
                TotalHoursNeeded = this.TotalHoursNeeded,
                TaskStatusId = this.TaskStatusId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUser = this.CreatedByUser,
                UpdatedByUser = this.UpdatedByUser,
                Username = this.Username
            };
        }

        public WorkScheduler.Model.Task MapToTaskPut()
        {
            return new WorkScheduler.Model.Task()
            {
                Description = this.Description,
                TotalHoursNeeded = this.TotalHoursNeeded,
                TaskStatusId = this.TaskStatusId,
                DateUpdated = DateTime.Now,
                UpdatedByUser = this.UpdatedByUser,
                Username = this.Username
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;

namespace WorkScheduler.WebAPI.Models
{
    public class ScheduleTaskWorkerRest
    {
        public Guid Id { get; set; }
        public string TaskDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int TaskDuration { get; set; }

        public ScheduleTaskWorkerRest() { }

        public ScheduleTaskWorkerRest(IScheduleTaskWorker scheduleTaskWorker, ITask task, IWorker worker)
        {
            this.Id = scheduleTaskWorker.Id;
            this.TaskDescription = task.Description;
            this.FirstName = worker.FirstName;
            this.LastName = worker.LastName;
            this.StartDateTime = scheduleTaskWorker.StartDateTime;
            this.EndDateTime = scheduleTaskWorker.EndDateTime;
            this.TaskDuration = scheduleTaskWorker.TaskDuration;
        }
    }
}
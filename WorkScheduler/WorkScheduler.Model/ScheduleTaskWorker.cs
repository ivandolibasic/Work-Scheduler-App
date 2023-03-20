using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class ScheduleTaskWorker: IScheduleTaskWorker
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid WorkerAvailabilityId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int TaskDuration { get; set; }
        public ScheduleTaskWorker()
        {
            this.Id = Guid.NewGuid();
        }
        public ScheduleTaskWorker(IScheduleTaskWorker ScheduleTaskWorker)
        {
            this.Id = Guid.NewGuid();
            this.TaskId = ScheduleTaskWorker.TaskId;
            this.WorkerAvailabilityId = ScheduleTaskWorker.WorkerAvailabilityId;
            this.StartDateTime = ScheduleTaskWorker.StartDateTime;
            this.EndDateTime = ScheduleTaskWorker.EndDateTime;
            this.TaskDuration = ScheduleTaskWorker.TaskDuration;
        }
    }
}

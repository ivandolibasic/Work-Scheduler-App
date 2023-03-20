using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IScheduleTaskWorker
    {
        Guid Id { get; set; }
        Guid TaskId { get; set; }
        Guid WorkerAvailabilityId { get; set; }
        DateTime StartDateTime { get; set; }
        DateTime EndDateTime { get; set; }
        int TaskDuration { get; set; }
    }
}

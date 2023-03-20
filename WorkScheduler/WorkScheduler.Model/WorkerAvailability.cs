using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class WorkerAvailability : IWorkerAvailability
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Guid RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public WorkerAvailability() 
        {
            this.Id = Guid.NewGuid();
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.MaxValue;
        }
        public WorkerAvailability(IWorkerAvailability workerAvailability)
        {
            this.Id = Guid.NewGuid();
            this.WorkerId = workerAvailability.WorkerId;
            this.RequestId = workerAvailability.RequestId;
            this.StartDate = workerAvailability.EndDate;
            this.EndDate = DateTime.MaxValue;
        }
    }
}

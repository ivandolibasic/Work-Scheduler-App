using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class WorkerStatus : IWorkerStatus
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}

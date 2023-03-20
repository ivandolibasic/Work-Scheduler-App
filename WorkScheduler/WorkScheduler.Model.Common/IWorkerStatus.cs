using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IWorkerStatus
    {
        Guid Id { get; set; }
        string Status { get; set; }
    }
}

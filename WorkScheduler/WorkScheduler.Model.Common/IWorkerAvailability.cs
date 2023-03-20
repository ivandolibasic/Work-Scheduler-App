using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IWorkerAvailability
    {
        Guid Id { get; set; }
        Guid WorkerId { get; set; }
        Guid RequestId { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}

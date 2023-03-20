using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Service.Common
{
    public interface IWorkerAvailabilityService
    {
        Task<IWorkerAvailability> GetAvailiabilityById(Guid id);
        Task<List<IWorkerAvailability>> GetAvailiabilityByWorker(Guid workerId);
        Task<string> UpdateAvailability(Guid workerId, IWorkerAvailability workerAvailability, Guid AdminId);
    }
}

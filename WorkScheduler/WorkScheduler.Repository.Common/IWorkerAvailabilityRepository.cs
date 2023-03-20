using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IWorkerAvailabilityRepository
    {
        Task<IWorkerAvailability> GetAvailabilityByIdAsync(Guid id);
        Task<List<IWorkerAvailability>> GetAvailabilityByWorkerAsync(Guid workerId);
        Task<List<IWorkerAvailability>> GetAvailableWorkerByWeekAsync(Guid workerId, DateTime date);
        Task<bool> CreateAvailability(IWorkerAvailability workerAvailability, IRequest request);
        Task<string> UpdateAvailabilityAsync(Guid workerId, IWorkerAvailability workerAvailability, Guid adminId);
        Task<string> UpdateAvailabilityByIdAsync(Guid id, IWorkerAvailability workerUnavailability, Guid AdminId);
    }
}

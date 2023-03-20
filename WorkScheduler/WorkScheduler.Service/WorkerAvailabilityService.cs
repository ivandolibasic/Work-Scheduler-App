using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class WorkerAvailabilityService : IWorkerAvailabilityService
    {
        private IWorkerAvailabilityRepository workerAvailabilityRepository;
        public WorkerAvailabilityService(IWorkerAvailabilityRepository workerAvailabilityRepository)
        {
            this.workerAvailabilityRepository = workerAvailabilityRepository;
        }

        public async Task<IWorkerAvailability> GetAvailiabilityById(Guid id)
        {
            return await workerAvailabilityRepository.GetAvailabilityByIdAsync(id);
        }
        public async Task<List<IWorkerAvailability>> GetAvailiabilityByWorker(Guid workerId)
        {
            return await workerAvailabilityRepository.GetAvailabilityByWorkerAsync(workerId);
        }
        public async Task<string> UpdateAvailability(Guid workerId, IWorkerAvailability workerAvailability, Guid AdminId)
        {
            return await workerAvailabilityRepository.UpdateAvailabilityAsync(workerId, workerAvailability, AdminId);
        }
    }
}

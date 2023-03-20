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
    public class WorkerStatusService:IWorkerStatusService
    {
        private IWorkerStatusRepository statusRepository;
        public WorkerStatusService(IWorkerStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }
        public async Task<List<IWorkerStatus>> GetAllAsync()
        {
            return await statusRepository.GetAllAsync();
        }
        public async Task<IWorkerStatus> GetAsync(Guid id)
        {
            return await statusRepository.GetAsync(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class WorkerService : IWorkerService
    {
        private IWorkerRepository workerRepository;
        private IWorkerAvailabilityRepository workerAvailabilityRepository;
        private IWorkerStatusRepository workerStatusRepository;
        private IRequestRepository requestRepository;
        public WorkerService(IWorkerRepository workerRepository, IWorkerAvailabilityRepository workerAvailabilityRepository, IWorkerStatusRepository workerStatusRepository, IRequestRepository requestRepository)
        {
            this.workerRepository = workerRepository;
            this.workerAvailabilityRepository = workerAvailabilityRepository;
            this.workerStatusRepository = workerStatusRepository;
            this.requestRepository = requestRepository;
        }
        public async Task<bool> Delete(Guid id)
        {
            List<IWorkerAvailability> workerAvailabilities = await workerAvailabilityRepository.GetAvailabilityByWorkerAsync(id);
            foreach(IWorkerAvailability workerAvailability in workerAvailabilities)
            {
                await requestRepository.DeleteRequestAsync(workerAvailability.RequestId);
            }
            return await workerRepository.Delete(id);
        }

        public async Task<List<IWorker>> Get(Sorting sorting, Paging paging, Filter filter)
        {
            return await workerRepository.Get(sorting, paging, filter);
        }

        public async Task<IWorker> Get(Guid id)
        {
            return await workerRepository.Get(id);
        }

     

        public async Task<IWorker> GetWorkerByAvailability(Guid workerAvailabilityId)
        {
            return await workerRepository.GetWorkerByAvailability(workerAvailabilityId);
        }
        public async Task<bool> Post(IWorker worker)
        {   var time = DateTime.Now;
            worker.DateCreated = time;
            worker.DateUpdated = time;
            IRequest request = new Request();
            request.MapRequest("Accepted", time, DateTime.MaxValue, "", await workerStatusRepository.GetAvailableIdAsync(), time, time, (Guid)worker.CreatedByUser, (Guid)worker.UpdatedByUser);
            IWorkerAvailability workerAvailability = new WorkerAvailability();
            workerAvailability.WorkerId = worker.Id;
            workerAvailability.RequestId = request.Id;
            if (await workerRepository.Post(worker))
                return await workerAvailabilityRepository.CreateAvailability(workerAvailability, request);
            return false;
        }

        public async Task<bool> Update(Guid id, IWorker worker)
        {
            worker.DateUpdated = DateTime.Now;
            return await workerRepository.Update(id,worker);
            
        }
    }
}

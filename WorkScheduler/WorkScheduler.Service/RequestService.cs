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
    public class RequestService : IRequestService
    {

        IWorkerAvailabilityRepository workerAvailabilityRepository;
        protected IRequestRepository Repository { get; private set; }
        public RequestService(IRequestRepository repository, IWorkerAvailabilityRepository workerAvailabilityRepository)
        {
            Repository = repository;
            workerAvailabilityRepository = workerAvailabilityRepository;
        }
        public async System.Threading.Tasks.Task CreateRequestAsync(IRequest request)
        {
            await Repository.CreateRequestAsync(request);
        }

        public async System.Threading.Tasks.Task DeleteRequestAsync(Guid id)
        {
            await Repository.DeleteRequestAsync(id);
        }

        public async Task<List<IRequest>> GetAllRequestsAsync(Sorting sorting, Paging paging)
        {
            return await Repository.GetAllRequestsAsync(sorting, paging);
        }

        public async Task<IRequest> GetOneRequestAsync(Guid id)
        {
            return await Repository.GetOneRequestAsync(id);
        }

        public async System.Threading.Tasks.Task UpdateRequestAsync(Guid id, IRequest request)
        {

            if (request.RequestStatus == "Aproved")
            {
                IWorkerAvailability workerAvailability = new WorkerAvailability();
                workerAvailability.WorkerId = request.CreatedByUser;
                workerAvailability.RequestId = id;
                workerAvailability.StartDate = request.StartDate;
                workerAvailability.EndDate = request.EndDate;
                if ("Worker not available at that time period!" == await workerAvailabilityRepository.UpdateAvailabilityAsync(request.CreatedByUser, workerAvailability, request.UpdatedByUser))
                {
                    return;
                }
            }
            await Repository.UpdateRequestAsync(id, request);
        }
    }
}

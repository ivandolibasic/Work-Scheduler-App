using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IRequestRepository
    {
        Task<List<IRequest>> GetAllRequestsAsync(Sorting sorting, Paging paging);
        Task<IRequest> GetOneRequestAsync(Guid id);
        Task CreateRequestAsync(IRequest request);
        Task<bool> CreateOnTaskWorkerAsync(Guid workerAvailabilityId, IRequest request, Guid adminId);
        Task UpdateRequestAsync(Guid id, IRequest request);
        Task DeleteRequestAsync(Guid id);
    }
}

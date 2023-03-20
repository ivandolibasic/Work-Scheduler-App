using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IWorkerRepository
    {
        Task<List<IWorker>> Get(Sorting sorting, Paging paging, Filter filter);
        Task<IWorker> Get(Guid id);
        Task<IWorker> GetWorkerByAvailability(Guid workerAvailabilityId);
        Task<bool> Delete(Guid id);
        Task<bool> Post(IWorker worker);
        Task<bool> Update(Guid id, IWorker worker);
    }
}

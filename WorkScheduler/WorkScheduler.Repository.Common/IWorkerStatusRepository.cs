using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IWorkerStatusRepository
    {
        Task<List<IWorkerStatus>> GetAllAsync();
        Task<IWorkerStatus> GetAsync(Guid id);
        Task<Guid> GetAvailableIdAsync();
        Task<Guid> GetOnTaskIdAsync();
    }
}

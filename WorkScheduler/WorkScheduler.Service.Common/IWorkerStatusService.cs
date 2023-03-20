using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Service.Common
{
    public interface IWorkerStatusService
    {
        Task<List<IWorkerStatus>> GetAllAsync();
        Task<IWorkerStatus> GetAsync(Guid id);
    }
}

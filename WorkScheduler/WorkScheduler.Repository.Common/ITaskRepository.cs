using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface ITaskRepository
    {
        Task<List<ITask>> GetAsync();
        Task<ITask> GetAsync(Guid id);
        Task<bool> PostAsync(ITask task);
        Task<bool> PutAsync(Guid id, ITask task);
        Task<bool> DeleteAsync(Guid id);
    }
}
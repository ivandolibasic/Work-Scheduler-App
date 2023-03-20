using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface ITaskStatusRepository
    {
        Task<List<ITaskStatus>> GetAsync();
        Task<ITaskStatus> GetAsync(Guid id);
        Task<ITaskStatus> GetAsync(string status);
    }
}
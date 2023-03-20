using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IScheduleTaskWorkerRepository
    {
        Task<List<IScheduleTaskWorker>> FindByWorker(Guid workerId, DateTime date);
        Task<List<IScheduleTaskWorker>> GetAllAsync();
        Task<string> AddToSchedule(IScheduleTaskWorker scheduleTaskWorker, Guid adminId);
        Task<bool> DeleteAsync(Guid id);
    }
}

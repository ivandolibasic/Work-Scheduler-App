using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Service.Common
{
    public interface IScheduleTaskWorkerService
    {
        Task<List<IScheduleTaskWorker>> FindByWorker(Guid workerId, DateTime date);
        Task<List<IScheduleTaskWorker>> GetAllAsync();
        Task<string> AddToSchedule(IScheduleTaskWorker scheduleTaskWorker, Guid adminId);
        Task<bool> DeleteAsync(Guid id);
    }
}

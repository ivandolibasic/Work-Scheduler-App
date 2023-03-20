using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class ScheduleTaskWorkerService:IScheduleTaskWorkerService
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        IScheduleTaskWorkerRepository scheduleTaskWorkerRepository;
        public ScheduleTaskWorkerService(IScheduleTaskWorkerRepository scheduleTaskWorkerRepository)
        {
            this.scheduleTaskWorkerRepository = scheduleTaskWorkerRepository;
        }
        public async Task<List<IScheduleTaskWorker>> FindByWorker(Guid workerId, DateTime date)
        {
            return await scheduleTaskWorkerRepository.FindByWorker(workerId, date);
        }
        public async Task<List<IScheduleTaskWorker>> GetAllAsync()
        {
            return await scheduleTaskWorkerRepository.GetAllAsync();
        }
        public async Task<string> AddToSchedule(IScheduleTaskWorker scheduleTaskWorker, Guid adminId)
        {
            return await scheduleTaskWorkerRepository.AddToSchedule(scheduleTaskWorker, adminId);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await scheduleTaskWorkerRepository.DeleteAsync(id);
        }
    }
}

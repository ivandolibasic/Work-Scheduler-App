using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class TaskStatusService : ITaskStatusService
    {
        private ITaskStatusRepository taskStatusRepository;

        public TaskStatusService(ITaskStatusRepository taskStatusRepository)
        {
            this.taskStatusRepository = taskStatusRepository;
        }

        public async Task<List<ITaskStatus>> GetAsync()
        {
            return await taskStatusRepository.GetAsync();
        }

        public async Task<ITaskStatus> GetAsync(Guid id)
        {
            return await taskStatusRepository.GetAsync(id);
        }

        public async Task<ITaskStatus> GetAsync(string status)
        {
            return await taskStatusRepository.GetAsync(status);
        }
    }
}
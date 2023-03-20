using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class TaskService : ITaskService
    {
        private ITaskRepository taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<List<ITask>> GetAsync()
        {
            return await taskRepository.GetAsync();
        }

        public async Task<ITask> GetAsync(Guid id)
        {
            return await taskRepository.GetAsync(id);
        }

        public async Task<bool> PostAsync(ITask task)
        {
            return await taskRepository.PostAsync(task);
        }

        public async Task<bool> PutAsync(Guid id, ITask task)
        {
            return await taskRepository.PutAsync(id, task);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await taskRepository.DeleteAsync(id);
        }

        //public void Initialize(ITaskRepository taskRepository)
        //{

        //}
    }
}
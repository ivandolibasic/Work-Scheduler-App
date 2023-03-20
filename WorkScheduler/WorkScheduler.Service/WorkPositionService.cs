using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    internal class WorkPositionService : IWorkPositionService
    {
        private IWorkPositionRepository workPositionRepository;
        public WorkPositionService(IWorkPositionRepository workPositionRepository)
        {
            this.workPositionRepository = workPositionRepository;
        }

        public async Task<bool> Delete(Guid id)
        {
            return await workPositionRepository.Delete(id) ;
        }

        public async Task<List<IWorkPosition>> Get(Sorting sorting, Paging paging)
        {
            return await workPositionRepository.Get(sorting,paging);
        }

        public async Task<IWorkPosition> Get(Guid id)
        {
            return await workPositionRepository.Get(id);
        }

        public async Task<bool> Post(IWorkPosition position)
        {
            var time = DateTime.UtcNow;
            position.DateCreated = time;
            position.DateUpdated = time;
            position.Id = Guid.NewGuid();
            return await workPositionRepository.Post(position);
        }

        public async Task<bool> Update(Guid id, IWorkPosition workPosition)
        {
            workPosition.DateUpdated = DateTime.UtcNow;
            return await workPositionRepository.Update(id, workPosition);
        }
    }
}

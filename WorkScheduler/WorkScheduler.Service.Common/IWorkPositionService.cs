using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Service.Common
{
    public interface IWorkPositionService
    {
        Task<List<IWorkPosition>> Get(Sorting sorting, Paging paging);
        Task<IWorkPosition> Get(Guid id);
        Task<bool> Post(IWorkPosition position);
        Task<bool> Update(Guid id, IWorkPosition workPosition);
        Task<bool> Delete(Guid id);
    }
}

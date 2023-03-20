using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IWorkPositionRepository
    {
        Task<List<IWorkPosition>> Get(Sorting sorting, Paging paging);
        Task<IWorkPosition> Get(Guid id);
        Task<bool> Post(IWorkPosition position);
        Task<bool> Update(Guid id, IWorkPosition workPosition);
        Task<bool> Delete(Guid id);

    }
}

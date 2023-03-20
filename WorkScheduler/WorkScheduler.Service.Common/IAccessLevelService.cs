using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Service.Common
{
    public interface IAccessLevelService
    {
        Task<List<IAccessLevel>> GetAll();
        Task<IAccessLevel> Get(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IAccessLevelRepository
    {
        Task<List<IAccessLevel>> GetAll();

        Task<IAccessLevel> Get(Guid id);
    }
}

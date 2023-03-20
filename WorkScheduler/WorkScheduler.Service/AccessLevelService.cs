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
    public class AccessLevelService:IAccessLevelService
    {
        private IAccessLevelRepository accessLevelRepository;
        public AccessLevelService(IAccessLevelRepository accessLevelRepository)
        {
            this.accessLevelRepository = accessLevelRepository;
        }

        public async Task<List<IAccessLevel>> GetAll()
        {
            return await accessLevelRepository.GetAll(); 
        }

        public async Task<IAccessLevel> Get(Guid id)
        {
            return await accessLevelRepository.Get(id);
        }
    }
}

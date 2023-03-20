using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Repository.Common
{
    public interface IAccountRepository
    {
        Task<List<IAccount>> GetAllAsync();
        Task<IAccount> FindAccountAsync(string username, string password);
        Task<IAccount> FindAccountByNameAsync(string username);
        Task<string> AddAccountAsync(IAccount account);
        Task<string> UpdateAccountAsync(Guid id, IAccount account);
        Task<bool> DeleteAccountAsync(Guid id);
    }
}

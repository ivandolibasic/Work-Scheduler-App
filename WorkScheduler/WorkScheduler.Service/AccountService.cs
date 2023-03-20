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
    class AccountService : IAccountService
    {
        private IAccountRepository accountRepository;
        private IWorkerRepository workerRepository;
        public AccountService(IAccountRepository accountRepository, IWorkerRepository workerRepository)
        {
            this.accountRepository = accountRepository;
            this.workerRepository = workerRepository;
        }

        public async Task<IAccount> FindAccountAsync(string username, string password)
        {
            return await accountRepository.FindAccountAsync(username, password);
        }

        public async Task<IAccount> FindAccountByNameAsync(string username)
        {
            return await accountRepository.FindAccountByNameAsync(username);
        }

        public async Task<List<IAccount>> GetAllAccounts()
        {
            return await accountRepository.GetAllAsync();
        }
        public async Task<string> AddAccountAsync(IAccount account)
        {
            return await accountRepository.AddAccountAsync(account);
        }

        public async Task<string> UpdateAccountAsync(Guid id, IAccount account)
        {
            return await accountRepository.UpdateAccountAsync(id, account);
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            return await accountRepository.DeleteAccountAsync(id);
        }


        public async Task<List<IAccount>> GetUsersWithoutWorkerProfile()
        {
            var sorting = new Sorting { OrderName = "LastName", OrderDirection = "ASC" };
            var paging = new Paging ();
            var filter = new Filter ();
            var allWorkers = await workerRepository.Get(sorting, paging, filter);
            var allUsers = await accountRepository.GetAllAsync();
            var usersWithoutWorkerProfile = new List<IAccount>();
            foreach (var user in allUsers)
            {
                if ((allWorkers.Exists(x => x.Id == user.Id)) == false)
                {
                    usersWithoutWorkerProfile.Add(user);
                }
            }
            return usersWithoutWorkerProfile;
        }

    }
}

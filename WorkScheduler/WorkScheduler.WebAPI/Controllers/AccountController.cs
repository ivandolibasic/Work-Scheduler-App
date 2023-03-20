using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;
using WorkScheduler.WebAPI.Models;

namespace WorkScheduler.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        private IAccountService accountService;
        private IAccessLevelService accessLevelService;
        public AccountController(IAccountService accountService, IAccessLevelService accessLevelService)
        {
            this.accountService = accountService;
            this.accessLevelService = accessLevelService;
        }

        public async Task<HttpResponseMessage> GetAsync(string username, string password)
        {
            if (username == null || password == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Missing username or password");
            }
            IAccount account = await accountService.FindAccountAsync(username, password);
            if (account.Username == null || account.Password == null) 
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Wrong username or password");
            }
            List<IAccessLevel> accessLevels = await accessLevelService.GetAll();
            AccountRest accountRest = new AccountRest();
            foreach (IAccessLevel accessLevel in accessLevels)
            {
                if(account.AccessLevelId==accessLevel.Id)
                {
                    accountRest = new AccountRest(account.Username, accessLevel.AccessLevelName);
                }
            }
            return Request.CreateResponse<AccountRest>(HttpStatusCode.OK, accountRest);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAccounts()
        {
            List<IAccessLevel> accessLevels = await accessLevelService.GetAll();
            List <IAccount> accounts= await accountService.GetAllAccounts();
            if (accounts != null && accessLevels != null)
            {
                List<AccountRest> accountRest = new List<AccountRest>();
                foreach (var account in accounts)
                {
                    accountRest.Add(AccountRest.MapToAccountRest(account, accessLevels));
                }
                return Request.CreateResponse(HttpStatusCode.OK, accountRest);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }


        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        [Route("api/UsersWithoutAccount")]
        public async Task<HttpResponseMessage> GetUsersWithoutWorkerProfile()
        {
            List<IAccount> accounts = await accountService.GetUsersWithoutWorkerProfile();
            if (accounts != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, accounts);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        [Route("api/AccessLevel")]
        public async Task<HttpResponseMessage> GetAllAccessLevels()
        {
            List<IAccessLevel> accessLevels = await accessLevelService.GetAll();
            if (accessLevels != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, accessLevels);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<HttpResponseMessage> PostAsync(AddAccountRest accountRest)
        {
            if (accountRest.Username == null || accountRest.Password == null || accountRest.AccessLevel == null) 
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Missing elements");
            }
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await accountService.FindAccountByNameAsync(identity.Name);
            IAccount account = new Account(accountRest.Username, accountRest.Password, accountRest.AccessLevel);
            account.CreatedByUser = userInfo.Id;
            account.UpdatedByUser = userInfo.Id;
            string responseMessage = await accountService.AddAccountAsync(account);
            if (responseMessage == "Account added")
            {
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Account/Admin")]
        public async Task<HttpResponseMessage> PostAdminAsync(AddAccountRest accountRest)
        {

            if (accountRest.Username == null || accountRest.Password == null || accountRest.AccessLevel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Missing elements");
            }
            List<IAccessLevel> accessLevels = await accessLevelService.GetAll(); 
            foreach(AccessLevel accessLevel in accessLevels)
            {
                if(accessLevel.Id==accountRest.AccessLevel)
                {
                    if (accessLevel.AccessLevelName == "SuperAdmin" || accessLevel.AccessLevelName == "Admin")  
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Unathorized request");
                    break;
                }
            }
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await accountService.FindAccountByNameAsync(identity.Name);
            IAccount account = new Account(accountRest.Username, accountRest.Password, accountRest.AccessLevel);
            account.CreatedByUser = userInfo.Id;
            account.UpdatedByUser = userInfo.Id;
            string responseMessage = await accountService.AddAccountAsync(account);
            if (responseMessage == "Account added")
            {
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<HttpResponseMessage> PutAsync(Guid id, AddAccountRest accountRest)
        {

            if (accountRest.Username == null || accountRest.Password == null || accountRest.AccessLevel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Missing elements");
            }
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await accountService.FindAccountByNameAsync(identity.Name);
            IAccount account = new Account(accountRest.Username, accountRest.Password, accountRest.AccessLevel);
            account.UpdatedByUser = userInfo.Id;
            string responseMessage = await accountService.UpdateAccountAsync(id, account);
            if (responseMessage == "Account updated")
            {
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/Accout/Admin")]
        public async Task<HttpResponseMessage> PutAdminAsync(Guid id, AddAccountRest accountRest)
        {

            if (accountRest.Username == null || accountRest.Password == null || accountRest.AccessLevel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Missing elements");
            }
            List<IAccessLevel> accessLevels = await accessLevelService.GetAll();
            foreach (AccessLevel accessLevel in accessLevels)
            {
                if (accessLevel.Id == accountRest.AccessLevel)
                {
                    if (accessLevel.AccessLevelName == "SuperAdmin" || accessLevel.AccessLevelName == "Admin")
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Unathorized request");
                    break;
                }
            }
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await accountService.FindAccountByNameAsync(identity.Name);
            IAccount account = new Account(accountRest.Username, accountRest.Password, accountRest.AccessLevel);
            account.UpdatedByUser = userInfo.Id;
            string responseMessage = await accountService.UpdateAccountAsync(id, account);
            if (responseMessage == "Account updated")
            {
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            if (await accountService.DeleteAccountAsync(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Account deleted");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found");
        }
    }
}
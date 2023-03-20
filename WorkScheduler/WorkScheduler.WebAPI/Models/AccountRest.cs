using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model.Common;

namespace WorkScheduler.WebAPI.Models
{
    public class AccountRest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string AccessLevel { get; set; }

        public AccountRest() { }
        public AccountRest (string username, string accessLevel)
        {
            this.Username = username;
            this.AccessLevel = accessLevel;
        }

        public static AccountRest MapToAccountRest(IAccount account, List<IAccessLevel> accessLevels)
        {
            var accountRest = new AccountRest();
            accountRest.Id = account.Id;
            accountRest.Username = account.Username;
            foreach (var accessLevel in accessLevels)
            {
                if (accessLevel.Id == account.AccessLevelId)
                {
                    accountRest.AccessLevel = accessLevel.AccessLevelName;
                }
            }
            return accountRest;
        }
    }
}
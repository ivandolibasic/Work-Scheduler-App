using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Service
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IAccountRepository accountRepository;
        private IAccessLevelRepository accessLevelRepository;
        public MyAuthorizationServerProvider(IAccountRepository accountRepository, IAccessLevelRepository accessLevelRepository)
        {
            this.accountRepository = accountRepository;
            this.accessLevelRepository = accessLevelRepository;
        }
        public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async System.Threading.Tasks.Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IAccount user = new Account();
            user = await accountRepository.FindAccountAsync(context.UserName, context.Password);
            IAccessLevel accessLevel = new AccessLevel();
            accessLevel = await accessLevelRepository.Get(user.AccessLevelId);
            if (user == null)
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, accessLevel.AccessLevelName));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(type: "Id", value: user.Id.ToString()));
            context.Validated(identity);

        }
    }
}

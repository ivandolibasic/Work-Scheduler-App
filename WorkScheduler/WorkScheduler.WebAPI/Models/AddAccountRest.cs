using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkScheduler.WebAPI.Models
{
    public class AddAccountRest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid AccessLevel { get; set; }


        public void Set(string username, string password, Guid accessLevel)
        {
            this.Username = username;
            this.Password = password;
            this.AccessLevel = accessLevel;
        }
    }
}
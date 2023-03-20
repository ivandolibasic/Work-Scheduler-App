using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class Account:IAccount
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid AccessLevelId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid UpdatedByUser { get; set; }

        public Account()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public Account(string username, string password, Guid AccessLevelId)
        {
            Id = Guid.NewGuid();
            this.Username = username;
            this.Password = password;
            this.AccessLevelId = AccessLevelId;
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
        }
    }
}

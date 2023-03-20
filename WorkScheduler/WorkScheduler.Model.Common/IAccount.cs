using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IAccount
    {
        Guid Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        Guid AccessLevelId { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        Guid CreatedByUser { get; set; }
        Guid UpdatedByUser { get; set; }
    }
}

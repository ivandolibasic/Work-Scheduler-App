using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IAccessLevel
    {
        Guid Id { get; set; }
        string AccessLevelName { get; set; }
    }
}

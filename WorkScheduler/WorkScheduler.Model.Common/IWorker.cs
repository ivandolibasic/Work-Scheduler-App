using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IWorker
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Guid? PositionId { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        Guid? CreatedByUser { get; set; }
        Guid? UpdatedByUser { get; set; }
        IWorkPosition workPosition { get; set; }
    }
}

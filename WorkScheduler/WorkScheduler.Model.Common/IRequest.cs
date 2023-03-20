using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduler.Model.Common
{
    public interface IRequest
    {
        Guid Id { get; set; }
        string RequestStatus { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string Description { get; set; }
        Guid WorkerStatusId { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        Guid CreatedByUser { get; set; }
        Guid UpdatedByUser { get; set; }
        void MapRequest(string requestStatus, DateTime startDate, DateTime endDate, string description, Guid workerStatusId, DateTime DateCreated, DateTime DateUpdated, Guid CreatedByUser, Guid UpdatedByUser);
    }
}

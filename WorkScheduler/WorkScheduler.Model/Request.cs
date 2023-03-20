using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{

    public class Request : IRequest
    {
        public Guid Id { get; set; }
        public string RequestStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid WorkerStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid UpdatedByUser { get; set; }

        public Request() { Id = Guid.NewGuid(); }

        public void MapRequest(string requestStatus, DateTime startDate, DateTime endDate, string description, Guid workerStatusId, DateTime DateCreated, DateTime DateUpdated, Guid CreatedByUser, Guid UpdatedByUser)
        {
            this.RequestStatus = requestStatus;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Description = description;
            this.WorkerStatusId = workerStatusId;
            this.DateCreated = DateCreated;
            this.DateUpdated = DateUpdated;
            this.CreatedByUser = CreatedByUser;
            this.UpdatedByUser = UpdatedByUser;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class Worker : IWorker
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? PositionId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid? CreatedByUser { get; set; }
        public Guid? UpdatedByUser { get; set; }
        public IWorkPosition workPosition { get; set; }
    }
}

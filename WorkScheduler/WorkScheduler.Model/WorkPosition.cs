using System;
using WorkScheduler.Model.Common;

namespace WorkScheduler.Model
{
    public class WorkPosition : IWorkPosition
    {
        public Guid Id { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid? CreatedByUser { get; set; }
        public Guid? UpdatedByUser { get; set; }
    }
}

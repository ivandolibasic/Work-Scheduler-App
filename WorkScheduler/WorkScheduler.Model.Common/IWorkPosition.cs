using System;

namespace WorkScheduler.Model.Common
{
    public interface IWorkPosition
    {
        Guid Id { get; set; }
        string PositionName { get; set; }
        string Description { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        Guid? CreatedByUser { get; set; }
        Guid? UpdatedByUser { get; set; }
    }
}

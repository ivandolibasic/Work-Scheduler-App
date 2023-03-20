using System;

namespace WorkScheduler.Model.Common
{
    public interface ITask
    {
        Guid Id { get; set; }
        string Description { get; set; }
        int TotalHoursNeeded { get; set; }
        Guid TaskStatusId { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        Guid CreatedByUser { get; set; }
        Guid UpdatedByUser { get; set; }
        string Username { get; set; }
    }
}
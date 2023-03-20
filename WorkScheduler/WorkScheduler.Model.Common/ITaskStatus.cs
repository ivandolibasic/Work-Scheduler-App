using System;

namespace WorkScheduler.Model.Common
{
    public interface ITaskStatus
    {
        Guid Id { get; set; }
        string Status { get; set; }
    }
}
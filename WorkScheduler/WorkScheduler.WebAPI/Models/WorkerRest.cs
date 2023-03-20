using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model.Common;

namespace WorkScheduler.WebAPI.Models
{
    public class WorkerRest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public WorkPositionRest workPosition { get; set; }

        public static WorkerRest MaptoWorkerRest(IWorker worker)
        {
            return new WorkerRest
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                workPosition = new WorkPositionRest
                {
                    Id = worker.workPosition.Id,
                    Description = worker.workPosition.Description,
                    PositionName = worker.workPosition.PositionName
                }
            };
        }
    }
}
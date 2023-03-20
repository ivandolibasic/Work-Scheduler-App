using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;

namespace WorkScheduler.WebAPI.Models
{
    public class WorkerPostRest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid PositionId { get; set; }


        public Worker MapToWorker()
        {
            return new Worker
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                PositionId = this.PositionId
            };
        }
    }
}
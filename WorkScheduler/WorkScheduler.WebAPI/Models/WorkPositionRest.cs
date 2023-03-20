using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model.Common;

namespace WorkScheduler.WebAPI.Models
{
    public class WorkPositionRest
    {
        public Guid Id { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }

        public static WorkPositionRest MapToWorkPositionRest(IWorkPosition workPosition)
        {
            return new WorkPositionRest
            {
                Id = workPosition.Id,
                PositionName = workPosition.PositionName,
                Description = workPosition.Description,
            };
        }
    }
}
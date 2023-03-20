using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduler.Model;

namespace WorkScheduler.WebAPI.Models
{
    public class WorkPositionPostRest
    {
        public string PositionName { get; set; }
        public string Description { get; set; }

        public WorkPosition MapToWorkPosition()
        {
            return new WorkPosition
            {
                PositionName = this.PositionName,
                Description = this.Description
            };
        }
    }
}
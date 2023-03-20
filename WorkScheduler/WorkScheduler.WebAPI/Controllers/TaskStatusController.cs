using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;
using WorkScheduler.WebAPI.Models;

namespace WorkScheduler.WebAPI.Controllers
{
    public class TaskStatusController : ApiController
    {
        private ITaskStatusService TaskStatusService { get; set; }

        public TaskStatusController(ITaskStatusService taskStatusService)
        {
            this.TaskStatusService = taskStatusService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync()
        {
            List<ITaskStatus> taskStatuses = await TaskStatusService.GetAsync();
            if (taskStatuses.Any())
            {
                List<TaskStatusRest> taskStatusesRest = new List<TaskStatusRest>();
                foreach (ITaskStatus taskStatus in taskStatuses)
                {
                    taskStatusesRest.Add(new TaskStatusRest(taskStatus.Id, taskStatus.Status));
                }
                return Request.CreateResponse(HttpStatusCode.OK, taskStatusesRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            ITaskStatus taskStatus = await TaskStatusService.GetAsync(id);
            if (taskStatus != null)
            {
                TaskStatusRest taskStatusRest = new TaskStatusRest(taskStatus.Id, taskStatus.Status);
                return Request.CreateResponse(HttpStatusCode.OK, taskStatusRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync(string status)
        {
            ITaskStatus taskStatus = await TaskStatusService.GetAsync(status);
            if (taskStatus != null)
            {
                TaskStatusRest taskStatusRest = new TaskStatusRest(taskStatus.Id, taskStatus.Status);
                return Request.CreateResponse(HttpStatusCode.OK, taskStatusRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;
using WorkScheduler.WebAPI.Models;

namespace WorkScheduler.WebAPI.Controllers
{
    public class ScheduleTaskWorkerController : ApiController
    {
        private IScheduleTaskWorkerService scheduleTaskWorkerService;
        private ITaskService taskService;
        private IWorkerService workerService;
        public ScheduleTaskWorkerController(IScheduleTaskWorkerService scheduleTaskWorkerService, ITaskService taskService, IWorkerService workerService)
        {
            this.scheduleTaskWorkerService = scheduleTaskWorkerService;
            this.taskService = taskService;
            this.workerService = workerService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> FindByWorker(Guid workerId, DateTime date)
        {
            List<IScheduleTaskWorker> scheduleTaskWorkers = new List<IScheduleTaskWorker>();
            List<ScheduleTaskWorkerRest> scheduleTaskWorkerRests = new List<ScheduleTaskWorkerRest>();
            scheduleTaskWorkers = await scheduleTaskWorkerService.FindByWorker(workerId, date);
            if (scheduleTaskWorkers.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "There are no entries present");
            }
            foreach (IScheduleTaskWorker scheduleTaskWorker in scheduleTaskWorkers)
            {
                ITask task = await taskService.GetAsync(scheduleTaskWorker.TaskId);
                IWorker worker = await workerService.GetWorkerByAvailability(scheduleTaskWorker.WorkerAvailabilityId);
                ScheduleTaskWorkerRest scheduleTaskWorkerRest = new ScheduleTaskWorkerRest(scheduleTaskWorker, task, worker);
                scheduleTaskWorkerRests.Add(scheduleTaskWorkerRest);
            }
            return Request.CreateResponse<List<ScheduleTaskWorkerRest>>(HttpStatusCode.OK, scheduleTaskWorkerRests);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync()
        {
            List<IScheduleTaskWorker> scheduleTaskWorkers = new List<IScheduleTaskWorker>();
            List<ScheduleTaskWorkerRest> scheduleTaskWorkerRests = new List<ScheduleTaskWorkerRest>();
            scheduleTaskWorkers = await scheduleTaskWorkerService.GetAllAsync();
            if (scheduleTaskWorkers.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, scheduleTaskWorkers);
            }
            foreach(IScheduleTaskWorker scheduleTaskWorker in scheduleTaskWorkers)
            {
                ITask task = await taskService.GetAsync(scheduleTaskWorker.TaskId);
                IWorker worker = await workerService.GetWorkerByAvailability(scheduleTaskWorker.WorkerAvailabilityId);
                ScheduleTaskWorkerRest scheduleTaskWorkerRest = new ScheduleTaskWorkerRest(scheduleTaskWorker, task, worker);
                scheduleTaskWorkerRests.Add(scheduleTaskWorkerRest);
            }
            return Request.CreateResponse<List<ScheduleTaskWorkerRest>>(HttpStatusCode.OK, scheduleTaskWorkerRests);
        }

        [HttpPost]
        [Route("api/schedule")]
        public async Task<HttpResponseMessage> AddToSchedule(IScheduleTaskWorker scheduleTaskWorker)
        {
            var identity = (ClaimsIdentity)User.Identity;
            Guid adminId = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            string responseMessage = await scheduleTaskWorkerService.AddToSchedule(scheduleTaskWorker, adminId);
            if (responseMessage == "Element added to Schedule")
            {
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage);
        }

        [HttpDelete]
        [Route("api/schedule")]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            if (await scheduleTaskWorkerService.DeleteAsync(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Entry deleted");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Entry not found");
        }
    }
}
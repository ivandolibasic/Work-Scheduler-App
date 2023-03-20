using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;
using WorkScheduler.WebAPI.Models;

namespace WorkScheduler.WebAPI.Controllers
{
    public class TaskController : ApiController
    {
        private ITaskService TaskService { get; set; }
        private ITaskStatusService TaskStatusService { get; set; }
        private IAccountService AccountService { get; set; }
        private IAccessLevelService AccessLevelService { get; set; }

        public TaskController(ITaskService taskService, ITaskStatusService taskStatusService, IAccountService accountService, IAccessLevelService accessLevelService)
        {
            this.TaskService = taskService;
            this.TaskStatusService = taskStatusService;
            this.AccountService = accountService;
            this.AccessLevelService = accessLevelService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync()
        {
            List<ITask> tasks = await TaskService.GetAsync();
            List<ITaskStatus> taskStatuses = await TaskStatusService.GetAsync();
            if (tasks.Any())
            {
                List<TaskRestGet> tasksRest = new List<TaskRestGet>();
                foreach (ITask task in tasks)
                {
                    string taskStatus = taskStatuses.FirstOrDefault(ts => ts.Id == task.TaskStatusId).Status;
                    if (taskStatus == null)
                    {
                        continue;
                    }
                    tasksRest.Add(new TaskRestGet(task.Id, task.Description, task.TotalHoursNeeded, task.DateCreated, task.DateUpdated, taskStatus, task.Username));
                }
                return Request.CreateResponse<List<TaskRestGet>>(HttpStatusCode.OK, tasksRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync([FromUri] Guid id)
        {
            ITask task = await TaskService.GetAsync(id);
            List<ITaskStatus> taskStatuses = await TaskStatusService.GetAsync();
            if (task != null)
            {
                TaskRestGet taskRest = null;
                foreach (ITaskStatus taskStatus in taskStatuses)
                {
                    if (task.TaskStatusId == taskStatus.Id)
                    {
                        taskRest = new TaskRestGet(task.Id, task.Description, task.TotalHoursNeeded, task.DateCreated, task.DateUpdated, taskStatus.Status, task.Username);
                    }
                }
                if (taskRest != null)
                {
                    return Request.CreateResponse<TaskRestGet>(HttpStatusCode.OK, taskRest);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> PostAsync([FromBody] TaskRestPostPut taskRestPost)
        {
            TaskRest taskRest = new TaskRest(taskRestPost.Description, taskRestPost.TotalHoursNeeded);
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await AccountService.FindAccountByNameAsync(identity.Name);
            ITaskStatus taskStatus = await TaskStatusService.GetAsync(taskRestPost.Status);
            if (userInfo != null && taskStatus != null)
            {
                taskRest.CreatedByUser = userInfo.Id;
                taskRest.UpdatedByUser = userInfo.Id;
                taskRest.Username = identity.Name;
                taskRest.TaskStatusId = taskStatus.Id;
                taskRest.Status = taskStatus.Status;
            }
            if (await TaskService.PostAsync(taskRest.MapToTaskPost()))
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id, [FromBody] TaskRestPostPut taskRestPut)
        {
            TaskRest taskRest = new TaskRest(id, taskRestPut.Description, taskRestPut.TotalHoursNeeded, taskRestPut.Status, taskRestPut.Username);
            var identity = (ClaimsIdentity)User.Identity;
            IAccount userInfo = await AccountService.FindAccountByNameAsync(identity.Name);
            ITaskStatus taskStatus = await TaskStatusService.GetAsync(taskRest.Status);
            if (userInfo != null && taskStatus != null)
            {
                taskRest.UpdatedByUser = userInfo.Id;
                taskRest.TaskStatusId = taskStatus.Id;
            }
            bool putResult = await TaskService.PutAsync(id, taskRest.MapToTaskPut());
            if (putResult)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id)
        {
            bool deleteResult = await TaskService.DeleteAsync(id);
            if (deleteResult)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
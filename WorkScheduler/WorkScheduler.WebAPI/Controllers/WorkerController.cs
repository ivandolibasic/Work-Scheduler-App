using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;
using WorkScheduler.WebAPI.Models;

namespace WorkScheduler.WebAPI.Controllers
{
    public class WorkerController : ApiController
    {
        private IWorkerService workerService;
        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }



        // GET: api/Worker
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Get([FromUri] Sorting sorting, [FromUri] Paging paging, [FromUri] Filter filter)
        {
            List<IWorker> workers = await workerService.Get(sorting,paging,filter);
            if (workers.Any())
            {
                List<WorkerRest> workerRests = new List<WorkerRest>();
                foreach (var worker in workers)
                {
                    workerRests.Add(WorkerRest.MaptoWorkerRest(worker));
                }
                return Request.CreateResponse(HttpStatusCode.OK, workerRests);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
        }

        // GET: api/Worker/id
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            IWorker worker = await workerService.Get(id);
            if (worker != null)
            {
                var workerRest = WorkerRest.MaptoWorkerRest(worker);
                return Request.CreateResponse(HttpStatusCode.OK, workerRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "There is no worker with id " + id);

        }

        // POST: api/Worker/
        [Authorize(Roles = "SuperAdmin, Admin, Worker")]
        public async Task<HttpResponseMessage> Post([FromBody]WorkerPostRest workerRest)
        {
            var worker = workerRest.MapToWorker();
            var identity = (ClaimsIdentity)User.Identity;
            var id = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            worker.CreatedByUser = id;
            worker.UpdatedByUser = id;
            if (await workerService.Post(worker))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Worker added in db");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        // PUT: api/Worker/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody]WorkerPostRest workerRest)
        {
            var worker = workerRest.MapToWorker();
            var identity = (ClaimsIdentity)User.Identity;
            var userid = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            worker.UpdatedByUser = userid;
            if (await workerService.Update(id, worker))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Worker updated");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        // DELETE: api/Worker/id
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            if (await workerService.Delete(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Worker deleted");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Deleting worker with id "+id+" failed");
        }
    }
}

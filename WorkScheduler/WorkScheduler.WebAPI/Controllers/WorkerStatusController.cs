using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.WebAPI.Controllers
{
    public class WorkerStatusController : ApiController
    {
        private IWorkerStatusService workerStatusService;
        public WorkerStatusController(IWorkerStatusService workerStatusService)
        {
            this.workerStatusService = workerStatusService;
        }
        [HttpGet]
        [Route("api/WorkerStatus/All")]
        public async Task<HttpResponseMessage> GetAllAsync()
        {
            List<IWorkerStatus> workerStatuses = await workerStatusService.GetAllAsync();
            if (workerStatuses.Count()==0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "List is empty");
            }
            return Request.CreateResponse(HttpStatusCode.OK, workerStatuses);
        }
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            IWorkerStatus workerStatus = await workerStatusService.GetAsync(id);
            if (workerStatus == null) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No worker status with that ID");
            }
            return Request.CreateResponse(HttpStatusCode.OK, workerStatus);
        }

    }
}
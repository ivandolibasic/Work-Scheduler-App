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
using WorkScheduler.Common;
using System.Security.Claims;

namespace WorkScheduler.WebAPI.Controllers
{
    public class WorkPositionController : ApiController
    {
        private IWorkPositionService workPositionService;
        public WorkPositionController(IWorkPositionService workPositionService)
        {
            this.workPositionService = workPositionService;
        }

        // GET: api/WorkPosition
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Get([FromUri]Sorting sorting, [FromUri]Paging paging)
        {
            List<IWorkPosition> workPositions = await workPositionService.Get(sorting,paging);
            if (workPositions.Any())
            {
                List<WorkPositionRest> positions = new List<WorkPositionRest>();
                foreach (var workPosition in workPositions)
                {
                    positions.Add(WorkPositionRest.MapToWorkPositionRest(workPosition));
                }
                return Request.CreateResponse(HttpStatusCode.OK,  positions);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "There are no work positions saved");
        }

        // GET: api/WorkPosition/id
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            IWorkPosition workPosition = await workPositionService.Get(id);
            if (workPosition != null)
            {
                WorkPositionRest workPositionRest = WorkPositionRest.MapToWorkPositionRest(workPosition);
                return Request.CreateResponse(HttpStatusCode.OK, workPositionRest);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "There is no work position with id = "+id);
        }

        // POST: api/WorkPosition
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Post([FromBody]WorkPositionPostRest workPositionRest)
        {
            var position = workPositionRest.MapToWorkPosition();
            var identity = (ClaimsIdentity)User.Identity;
            var id = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            position.CreatedByUser = id;
            position.UpdatedByUser = id;
            if (await workPositionService.Post(position))
            {
                return Request.CreateResponse(HttpStatusCode.Created, "Worker position added in db");
            }
            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Not acceptable");
        }

        // PUT: api/WorkPosition/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody]WorkPositionPostRest workPositionRest)
        {
            var position = workPositionRest.MapToWorkPosition();
            var identity = (ClaimsIdentity)User.Identity;
            var userId = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            position.UpdatedByUser = userId;
            if (await workPositionService.Update(id, position))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Work position updated");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        // DELETE: api/WorkPosition/id
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            var response = await workPositionService.Delete(id);
            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Work position deleted");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Can't delete work position");
        }
    }
}

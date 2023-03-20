using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;
using WorkScheduler.Common;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Service.Common;

namespace WorkScheduler.WebAPI.Controllers
{
    public class RequestController : ApiController
    {
        protected IRequestService Service { get; private set; }
        public RequestController(IRequestService service)
        {
            Service = service;
        }

        public async Task<HttpResponseMessage> GetAllRequestes([FromUri] Sorting sorting, [FromUri] Paging paging)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await Service.GetAllRequestsAsync(sorting, paging));
        }

        public async Task<HttpResponseMessage> GetOneRequest(Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await Service.GetOneRequestAsync(id));
        }

        public async Task<HttpResponseMessage> CreateRequestAsync([FromBody]Request request)
        {
            await Service.CreateRequestAsync(request);
            return Request.CreateResponse(HttpStatusCode.OK, "Request created!");
        }

        public async Task<HttpResponseMessage> UpdateRequestAsync(Guid id, [FromBody]Request request)
        {
            await Service.UpdateRequestAsync(id, request);
            return Request.CreateResponse(HttpStatusCode.OK, "Request updated!");
        }

        public async Task<HttpResponseMessage> DeleteRequestAsync(Guid id)
        {
            await Service.DeleteRequestAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Request deleted!");
        }
    }
}

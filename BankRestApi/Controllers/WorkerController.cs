using BankService.BindingModels;
using BankService.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankRestApi.Controllers
{
    [Authorize]
    public class WorkerController : ApiController
    {
        private readonly IWorkerService service;

        public WorkerController(IWorkerService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetList()
        {
            var list = await service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var element = await service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public async Task AddElement(WorkerBindingModel model)
        {
            await service.AddElement(model);
        }

        [HttpPut]
        public async Task UpdElement(WorkerBindingModel model)
        {
            await service.UpdElement(model);
        }

        [HttpDelete]
        public async Task DelElement(int id)
        {
            await service.DelElement(id);
        }

        [HttpPut]
        public async Task UpBonus(CalcBindingModel model)
        {
            await service.UpBonus(model);
        }

        [HttpPut]
        public async Task DownBonus(CalcBindingModel model)
        {
            await service.DownBonus(model);
        }
    }
}

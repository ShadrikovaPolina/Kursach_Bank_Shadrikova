using BankService.BindingModels;
using BankService.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BankRestApi.Controllers
{
    [Authorize]
    public class AdminController : ApiController
    {
        private readonly IAdminService service;

        private AppUserManager _userManager;

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AdminController(IAdminService service)
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
        public async Task AddElement(AdminBindingModel model)
        {
            await UserManager.CreateAsync(model, model.PasswordHash);
        }

        [HttpPut]
        public async Task UpdElement(AdminBindingModel model)
        {
            await service.UpdElement(model);
        }

        [HttpDelete]
        public async Task DelElement(int id)
        {
            await service.DelElement(id);
        }
    }
}

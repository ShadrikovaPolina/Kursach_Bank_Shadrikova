using BankService.BindingModels;
using BankService.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankRestApi.Controllers
{
    [Authorize]
    public class ReportController : ApiController
    {
        private readonly IReportService service;

        private readonly string TempPath;

        private readonly string ResourcesPath;

        public ReportController(IReportService service)
        {
            this.service = service;
            TempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/");
            ResourcesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/");
        }

        [HttpPost]
        public async Task SendWorkersSalaryDoc(ReportBindingModel model)
        {
            model.FileName = TempPath;
            await service.SendWorkersSalaryDoc(model);
        }

        [HttpPost]
        public async Task SendWorkersSalaryXls(ReportBindingModel model)
        {
            model.FileName = TempPath;
            await service.SendWorkersSalaryXls(model);
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetAccounts(ReportBindingModel model)
        {
            var list = await service.GetAccounts(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task SaveWorkersSalaryAccount(ReportBindingModel model)
        {
            model.FontPath = ResourcesPath + "TIMCYR.TTF";
            if (!File.Exists(model.FontPath))
            {
                File.WriteAllBytes(model.FontPath, Properties.Resources.TIMCYR);
            }
            await service.SaveWorkersSalaryAccount(model);
        }
    }
}

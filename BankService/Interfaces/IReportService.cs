using BankService.BindingModels;
using BankService.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankService.Interfaces
{
    public interface IReportService
    {
        Task SendWorkersSalaryDoc(ReportBindingModel model);

        Task SendWorkersSalaryXls(ReportBindingModel model);

        Task SendMail(string mailto, string caption, string message, string path = null);

        Task<List<AccountViewModel>> CalcSalaries();

        Task<List<AccountViewModel>> GetAccounts(ReportBindingModel model);

        Task SaveWorkersSalaryAccount(ReportBindingModel model);
    }
}

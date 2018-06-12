using BankService.BindingModels;
using BankService.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankService.Interfaces
{
    public interface IWorkerService
    {
        Task<List<WorkerViewModel>> GetList();

        Task<WorkerViewModel> GetElement(int id);

        Task AddElement(WorkerBindingModel model);

        Task UpdElement(WorkerBindingModel model);

        Task DelElement(int id);

        Task UpBonus(CalcBindingModel model);

        Task DownBonus(CalcBindingModel model);
    }
}
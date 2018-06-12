using BankService.App;
using BankService.BindingModels;
using BankService.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankService.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminViewModel>> GetList();

        Task<AdminViewModel> GetElement(int id);

        Task<AppUser> GetUser(int id);

        Task<AppUser> GetUserByName(string name);

        Task AddElement(AdminBindingModel model);

        Task UpdElement(AdminBindingModel model);

        Task DelElement(int id);
    }
}
using BankService.BindingModels;
using BankService.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace BankService.App
{
    public class AppUserStore : UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        private readonly IAdminService serviceA;

        public AppUserStore(AbstractDbContext context, IAdminService serviceA) : base(context)
        {
            this.serviceA = serviceA;
        }

        public override Task<AppUser> FindByIdAsync(int userId)
        {
            return serviceA.GetUser(userId);
        }

        public async override Task CreateAsync(AppUser user)
        {
            await serviceA.AddElement(user as AdminBindingModel);
        }

        public override Task DeleteAsync(AppUser user)
        {
            return serviceA.DelElement(user.Id);
        }

        public async override Task<AppUser> FindByNameAsync(string userName)
        {
            AppUser user = null;

            if ((user = await serviceA.GetUserByName(userName)) != null)
            {
                return user;
            }
            return null;
        }

        public override Task UpdateAsync(AppUser user)
        {
            return serviceA.UpdElement(user as AdminBindingModel);
        }
    }
}

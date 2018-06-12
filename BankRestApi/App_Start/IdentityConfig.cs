using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Security.Claims;
using System.Collections.Generic;
using BankService.ImpBD;
using BankService;
using BankService.App;

namespace BankRestApi
{
    // Настройка диспетчера пользователей приложения. UserManager определяется в ASP.NET Identity и используется приложением.

    public class AppUserManager : UserManager<BankService.App.AppUser, int>
    {
        public AppUserManager(IUserStore<BankService.App.AppUser, int> store)
            : base(store)
        {
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(BankService.App.AppUser user, string authenticationType)
        {
            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection, authenticationType);

            return await Task.Run(() => claimsIdentity);
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new AppUserStore(context.Get<AbstractDbContext>(), AdminServiceBD.Create(context.Get<AbstractDbContext>())));
            // Настройка логики проверки имен пользователей
            manager.UserValidator = new UserValidator<BankService.App.AppUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Настройка логики проверки паролей
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}

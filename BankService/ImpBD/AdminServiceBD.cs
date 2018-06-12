using BankModel;
using BankService.App;
using BankService.BindingModels;
using BankService.Interfaces;
using BankService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.ImpBD
{
    public class AdminServiceBD : IAdminService
    {
        private AbstractDbContext context;

        public AdminServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public static AdminServiceBD Create(AbstractDbContext context)
        {
            return new AdminServiceBD(context);
        }

        public async Task AddElement(AdminBindingModel model)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec => rec.AdminFIO == model.AdminFIO);
            if (element != null)
            {
                throw new Exception("Уже есть админ с таким именем!");
            }
            element = await context.Admins.FirstOrDefaultAsync(rec => rec.UserName == model.UserName);
            if (element != null)
            {
                throw new Exception("Уже есть админ с таким логином!");
            }
            context.Admins.Add(new Admin
            {
                AdminFIO = model.AdminFIO,
                UserName = model.UserName,
                PasswordHash = model.PasswordHash,
                SecurityStamp = model.SecurityStamp
            });
            context.Configuration.ValidateOnSaveEnabled = false;

            await context.SaveChangesAsync();
        }

        public async Task DelElement(int id)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                context.Admins.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<AdminViewModel> GetElement(int id)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                return new AdminViewModel
                {
                    Id = element.Id,
                    AdminFIO = element.AdminFIO,
                    UserName = element.UserName
                };
            }
            throw new Exception("Элемент не найден!");
        }

        public async Task<List<AdminViewModel>> GetList()
        {
            List<AdminViewModel> result = await context.Admins.Select(rec => new AdminViewModel
            {
                Id = rec.Id,
                AdminFIO = rec.AdminFIO,
                UserName = rec.UserName
            })
                 .ToListAsync();
            return result;
        }

        public async Task<AppUser> GetUser(int id)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                return new AppUser
                {
                    Id = element.Id,
                    UserName = element.UserName,
                    PasswordHash = element.PasswordHash,
                    SecurityStamp = element.SecurityStamp
                };
            }
            return null;
        }

        public async Task<AppUser> GetUserByName(string name)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec => rec.UserName.Equals(name));
            if (element != null)
            {
                return new AppUser
                {
                    Id = element.Id,
                    UserName = element.UserName,
                    PasswordHash = element.PasswordHash,
                    SecurityStamp = element.SecurityStamp
                };
            }
            return null;
        }

        public async Task UpdElement(AdminBindingModel model)
        {
            Admin element = await context.Admins.FirstOrDefaultAsync(rec =>
                                    rec.AdminFIO == model.AdminFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть админ с таким именем");
            }
            element = context.Admins.FirstOrDefault(rec => rec.UserName == model.UserName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть админ с таким логином");
            }
            element = await context.Admins.FirstOrDefaultAsync(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.AdminFIO = model.AdminFIO;
            element.UserName = model.UserName;
            element.PasswordHash = model.PasswordHash;
            element.SecurityStamp = model.SecurityStamp;
            await context.SaveChangesAsync();
        }
    }
}

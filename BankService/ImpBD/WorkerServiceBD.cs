using BankModel;
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
    public class WorkerServiceBD : IWorkerService
    {
        private AbstractDbContext context;

        public WorkerServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public async Task AddElement(WorkerBindingModel model)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec => rec.WorkerFIO == model.WorkerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким именем");
            }
            context.Workers.Add(new Worker
            {
                WorkerFIO = model.WorkerFIO,
                DateCreate = DateTime.Now,
                Bonus = 0
            });
            context.SaveChanges();
        }

        public async Task DownBonus(CalcBindingModel model)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec => rec.Id == model.WorkerId);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Bonus = (element.Bonus - model.Fine > 0) ? element.Bonus - model.Fine : 0;
            await context.SaveChangesAsync();
        }

        public async Task DelElement(int id)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                context.Workers.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<WorkerViewModel> GetElement(int id)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                return new WorkerViewModel
                {
                    Id = element.Id,
                    WorkerFIO = element.WorkerFIO,
                    Bonus = element.Bonus
                };
            }
            throw new Exception("Элемент не найден");
        }

        public async Task<List<WorkerViewModel>> GetList()
        {
            List<WorkerViewModel> result = await context.Workers.Select(rec => new WorkerViewModel
            {
                Id = rec.Id,
                WorkerFIO = rec.WorkerFIO,
                Bonus = rec.Bonus
            })
                 .ToListAsync();
            return result;
        }

        public async Task UpBonus(CalcBindingModel model)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec => rec.Id == model.WorkerId);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Bonus += model.Bonus;
            await context.SaveChangesAsync();
        }

        public async Task UpdElement(WorkerBindingModel model)
        {
            Worker element = await context.Workers.FirstOrDefaultAsync(rec =>
                                   rec.WorkerFIO == model.WorkerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с такими данными");
            }
            element = await context.Workers.FirstOrDefaultAsync(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WorkerFIO = model.WorkerFIO;
            await context.SaveChangesAsync();
        }
    }
}

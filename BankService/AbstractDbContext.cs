using BankModel;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BankService
{
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext() : base("BankDB")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public static AbstractDbContext Create()
        {
            return new AbstractDbContext();
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Worker> Workers { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }
    }
}

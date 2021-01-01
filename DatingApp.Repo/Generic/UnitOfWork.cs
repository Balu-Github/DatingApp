using DatingApp.Contracts.Generic;
using DatingApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Repo.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        public DatingAppContext Context { get; }

        public UnitOfWork(DatingAppContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
           await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

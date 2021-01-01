using DatingApp.Data;
using System;
using System.Threading.Tasks;

namespace DatingApp.Contracts.Generic
{
    public interface IUnitOfWork : IDisposable
    {

        DatingAppContext Context { get; }

        void Commit();

        Task CommitAsync();
    }
}

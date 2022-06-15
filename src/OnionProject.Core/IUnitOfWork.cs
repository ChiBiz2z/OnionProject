using OnionProject.Core.Repositories;
using System.Data;

namespace OnionProject.Core
{
    public interface IUnitOfWork : IDisposable
    {
        //Interface(s) Name(s) {get;}
        Task<int> CompleteAsync();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel level);
        void RollbackTransaction();
        void CommitTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}

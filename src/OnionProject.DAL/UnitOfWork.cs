using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnionProject.Core;
using OnionProject.Core.Repositories;
using OnionProject.DAL.Repositories;
using System.Collections.Concurrent;
using System.Data;

namespace OnionProject.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly ConcurrentDictionary<Type, object> repositories;
        private IDbContextTransaction? _transaction;
        private bool disposed;


        //Interfaces

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            repositories = new ConcurrentDictionary<Type, object>();
            //Interfaces = new Repositories();
        }

        public Task<int> CompleteAsync()
        {
            return context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel level)
        {
            _transaction = context.Database.BeginTransaction(level);
        }

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();

            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();

            _transaction = null;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return repositories.GetOrAdd(typeof(TEntity), new Repository<TEntity>(context)) as IRepository<TEntity>;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                context.Dispose();

            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}

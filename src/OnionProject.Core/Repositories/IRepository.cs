using System.Linq.Expressions;

namespace OnionProject.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        TEntity GetById(int id);
        IQueryable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(int id);
        void RemoveRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}

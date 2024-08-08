using System.Linq.Expressions;

namespace Business.Repository
{
    public interface IRepository<T> where T : class
    {
        List<T> Getall(Expression<Func<T, bool>> result = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> result = null, bool tracked = true, string? includeProperties = null);
        void Create(T entity);
        void Delete(T entity);
        void Save();
    }
}

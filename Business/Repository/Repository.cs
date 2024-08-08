using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskManagerContext _context;
        internal DbSet<T> _dbSet;

        public Repository(TaskManagerContext context)
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            Save();
        }

        public T Get(Expression<Func<T, bool>> result = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (tracked == false)
            {
                query = query.AsNoTracking();
            }
            if (result != null)
            {
                query = query.Where(result);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();

        }

        public List<T> Getall(Expression<Func<T, bool>> result = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (result != null)
            {
                query = query.Where(result);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

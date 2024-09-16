using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain.IRepositories;

namespace P7CreateRestApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LocalDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(LocalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public  IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return  _dbSet.Find(id);
        }

        public void Add(T entity)
        {
             _dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            _dbSet.Remove(entity);
        }
    }
}

﻿using System.Threading.Tasks;

namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
    }
}
    
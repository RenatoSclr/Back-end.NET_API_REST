namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task UpdateAsync(Rating entity);
        Task SaveAsync();
    }
}

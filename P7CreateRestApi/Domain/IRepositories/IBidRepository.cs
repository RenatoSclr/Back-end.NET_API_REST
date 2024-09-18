namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IBidRepository : IRepository<Bid>
    {
        Task UpdateAsync(Bid entity);
        Task SaveAsync();
    }
}

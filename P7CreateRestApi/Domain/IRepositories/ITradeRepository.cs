namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface ITradeRepository : IRepository<Trade>
    {
        Task UpdateAsync(Trade entity);
        Task SaveAsync();
    }
}

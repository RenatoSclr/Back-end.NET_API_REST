namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface ICurvePointRepository : IRepository<Bid>
    {
        Task UpdateAsync(Bid entity);
        Task SaveAsync();
    }
}

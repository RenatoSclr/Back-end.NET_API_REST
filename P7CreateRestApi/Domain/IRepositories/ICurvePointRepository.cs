namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface ICurvePointRepository : IRepository<CurvePoint>
    {
        Task UpdateAsync(CurvePoint entity);
        Task SaveAsync();
    }
}

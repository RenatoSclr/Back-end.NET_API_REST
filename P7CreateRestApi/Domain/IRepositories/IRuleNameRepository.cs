namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IRuleNameRepository : IRepository<RuleName>
    {
        Task UpdateAsync(RuleName entity);
        Task SaveAsync();
    }
}

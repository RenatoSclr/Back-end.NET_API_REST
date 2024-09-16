namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IBidRepository : IRepository<BidList>
    {
        void Update(BidList entity);
        void Save();
    }
}

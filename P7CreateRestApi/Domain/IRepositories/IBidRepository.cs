namespace Dot.Net.WebApi.Domain.IRepositories
{
    public interface IBidRepository : IRepository<Bid>
    {
        void Update(Bid entity);
        void Save();
    }
}

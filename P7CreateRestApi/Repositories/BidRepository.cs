using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository : Repository<BidList>, IBidRepository
    {
        private readonly LocalDbContext _context;
        public BidRepository(LocalDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(BidList entity)
        {
            _context.Update(entity);
        }
    }
}

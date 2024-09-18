using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        private readonly LocalDbContext _context;
        public BidRepository(LocalDbContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateAsync(Bid entity)
        {
            _context.Update(entity);
            await SaveAsync(); 
        }
    }
}

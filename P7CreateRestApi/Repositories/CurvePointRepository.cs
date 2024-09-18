using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository : Repository<Bid>, ICurvePointRepository
    {
        private readonly LocalDbContext _context;
        public CurvePointRepository(LocalDbContext context) : base(context)
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

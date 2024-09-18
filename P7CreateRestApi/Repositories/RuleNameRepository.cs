using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class RuleNameRepository : Repository<RuleName>, IRuleNameRepository
    {
        private readonly LocalDbContext _context;
        public RuleNameRepository(LocalDbContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateAsync(RuleName entity)
        {
            _context.Update(entity);
            await SaveAsync(); 
        }
    }
}

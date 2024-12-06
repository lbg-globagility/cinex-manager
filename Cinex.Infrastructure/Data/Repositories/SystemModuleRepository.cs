using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class SystemModuleRepository : SavableRepository<SystemModule>, ISystemModuleRepository
    {
        public SystemModuleRepository(CinexContext context) :
            base(context)
        {
        }

        public SystemModule GetByCode(string code, string desc = "", string group = "")
        {
            var query = _context.SystemModules
            .AsNoTracking()
            .Where(t => t.Code == code)
            .AsQueryable();

            if (!string.IsNullOrEmpty(desc))
                query = query.Where(t => t.Description == desc);

            if (!string.IsNullOrEmpty(group))
                query = query.Where(t => t.Group == group);

            return query.AsEnumerable().FirstOrDefault();
        }

        public async Task<SystemModule> GetByCodeAsync(string code, string desc = "", string group = "")
        {
            var query = _context.SystemModules
            .AsNoTracking()
            .Where(t => t.Code == code)
            .AsQueryable();

            if (!string.IsNullOrEmpty(desc))
                query = query.Where(t => t.Description == desc);

            if (!string.IsNullOrEmpty(group))
                query = query.Where(t => t.Group == group);

            return await query.FirstOrDefaultAsync();
        }
    }
}

using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class PatronRepository : SavableRepository<Patron>, IPatronRepository
    {
        public PatronRepository(CinexContext context) :
            base(context)
        {
        }

        public override async Task<ICollection<Patron>> GetAllAsync() => await _context
            .Patrons
            .Include(p => p.TicketPrice)
            .AsNoTracking()
            .ToListAsync();
    }
}

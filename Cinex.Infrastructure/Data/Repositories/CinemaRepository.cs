using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class CinemaRepository : SavableRepository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(CinexContext context) : base(context) { }

        public override async Task<Cinema> GetByIdAsync(int id) => await _context.Cinemas
            .Include(c => c.Patrons)
            .Include(c => c.DefaultPatrons)
            .Include(c => c.SoundSystem)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}

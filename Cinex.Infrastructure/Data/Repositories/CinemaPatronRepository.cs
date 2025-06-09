using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class CinemaPatronRepository : SavableRepository<CinemaPatron>, ICinemaPatronRepository
    {
        public CinemaPatronRepository(CinexContext context) : base(context)
        {
        }
    }
}

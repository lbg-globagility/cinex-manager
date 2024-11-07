using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class CinemaPatronDefaultRepository : SavableRepository<CinemaPatronDefault>, ICinemaPatronDefaultRepository
    {
        public CinemaPatronDefaultRepository(CinexContext context) : base(context)
        {
        }
    }
}

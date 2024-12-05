using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class EwalletRepository : SavableRepository<Ewallet>, IEwalletRepository
    {
        public EwalletRepository(CinexContext context) : base(context) { }
    }
}

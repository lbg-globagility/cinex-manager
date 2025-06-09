using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class SessionEwalletRepository : SavableRepository<SessionEwallet>, ISessionEwalletRepository
    {
        public SessionEwalletRepository(CinexContext context) :
            base(context)
        {
        }
    }
}

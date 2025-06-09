using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class AuditTrailRepository : SavableRepository<AuditTrail>, IAuditTrailRepository
    {
        public AuditTrailRepository(CinexContext context) : base(context)
        {
        }
    }
}

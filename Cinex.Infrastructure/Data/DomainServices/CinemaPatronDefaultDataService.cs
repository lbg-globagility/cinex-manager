using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class CinemaPatronDefaultDataService : BaseSavableDataService<CinemaPatronDefault>, ICinemaPatronDefaultDataService
    {
        public CinemaPatronDefaultDataService(
            ICinemaPatronDefaultRepository cinemaPatronDefaultRepository,
            IAuditTrailRepository auditTrailRepository,
            CinexContext context) : base(cinemaPatronDefaultRepository, auditTrailRepository, context)
        {
        }
    }
}

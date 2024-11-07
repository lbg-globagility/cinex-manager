using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class CinemaPatronDataService : BaseSavableDataService<CinemaPatron>, ICinemaPatronDataService
    {
        public CinemaPatronDataService(ICinemaPatronRepository cinemaPatronRepository,
            IAuditTrailRepository auditTrailRepository,
            CinexContext context) :
            base(cinemaPatronRepository,
                auditTrailRepository,
                context)
        {
        }
    }
}

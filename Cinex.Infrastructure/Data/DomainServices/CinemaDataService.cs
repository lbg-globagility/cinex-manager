using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class CinemaDataService : BaseSavableDataService<Cinema>, ICinemaDataService
    {
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaDataService(
            ICinemaRepository cinemaRepository,
            IAuditTrailRepository auditTrailRepository,
            CinexContext context) :
            base(cinemaRepository, auditTrailRepository, context)
        {
            _cinemaRepository = cinemaRepository;
        }
    }
}

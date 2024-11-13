using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class CinemaDataService : BaseSavableDataService<Cinema>, ICinemaDataService
    {
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaDataService(
            ICinemaRepository cinemaRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(
                cinemaRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<ICollection<Cinema>> GetAllAsync() => await _cinemaRepository.GetAllAsync();

        protected override int ModuleCodeId(Cinema entity = null) => 1;

        protected override string TableName(Cinema entity = null) => Cinema.TABLE_NAME;
    }
}

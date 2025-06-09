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

        protected override int ModuleCodeId(Cinema entity = null)
        {
            if (entity?.IsNewEntity ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_ADD_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsEdited ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_EDIT_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsDelete ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_DELETE_CODE_TEXT)?.Id ?? 0;
            return 0;
        }

        protected override string TableName(Cinema entity = null) => Cinema.TABLE_NAME;
    }
}

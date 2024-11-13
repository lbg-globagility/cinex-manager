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
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(cinemaPatronRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
        }

        protected override int ModuleCodeId(CinemaPatron entity = null)
        {
            if (entity?.IsNewEntity ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_ADD_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsEdited ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_EDIT_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsDelete ?? false) return _systemModuleRepository.GetByCode(SystemModule.CINEMA_DELETE_CODE_TEXT)?.Id ?? 0;
            return 0;
        }

        protected override string TableName(CinemaPatron entity = null) => CinemaPatron.TABLE_NAME;
    }
}

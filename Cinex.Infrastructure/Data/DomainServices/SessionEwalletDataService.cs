using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class SessionEwalletDataService : BaseSavableDataService<SessionEwallet>, ISessionEwalletDataService
    {
        private readonly ISessionEwalletRepository _sessionEwalletRepository;

        public SessionEwalletDataService(ISessionEwalletRepository sessionEwalletRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(sessionEwalletRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _sessionEwalletRepository = sessionEwalletRepository;
        }

        public async Task<ICollection<SessionEwallet>> GetAllAsync() => await _sessionEwalletRepository.GetAllAsync();

        protected override int ModuleCodeId(SessionEwallet entity = null)
        {
            var description = "RESERVE TICKET";
            if (entity?.IsNewEntity ?? false) return _systemModuleRepository.GetByCode(SystemModule.RESERVE_CODE_TEXT, desc: description)?.Id ?? 0;
            if (entity?.IsEdited ?? false) return _systemModuleRepository.GetByCode(SystemModule.RESERVE_CODE_TEXT, desc: description)?.Id ?? 0;
            if (entity?.IsDelete ?? false) return _systemModuleRepository.GetByCode(SystemModule.RESERVE_CODE_TEXT, desc: description)?.Id ?? 0;
            return 0;
        }

        protected override string TableName(SessionEwallet entity = null) => SessionEwallet.TABLE_NAME;
    }
}

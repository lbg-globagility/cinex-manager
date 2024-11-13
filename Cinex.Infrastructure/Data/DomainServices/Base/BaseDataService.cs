using Cinex.Core.Interfaces.DomainServices.Base;
using Cinex.Core.Interfaces.Repositories;

namespace Cinex.Infrastructure.Data.DomainServices.Base
{
    public abstract class BaseDataService : IBaseDataService
    {
        public readonly IAuditTrailRepository _auditTrailRepository;
        public readonly ISystemModuleRepository _systemModuleRepository;

        public BaseDataService(IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository)
        {
            _auditTrailRepository = auditTrailRepository;
            _systemModuleRepository = systemModuleRepository;
        }
    }
}

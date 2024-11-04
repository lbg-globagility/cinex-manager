using Cinex.Core.Interfaces.DomainServices.Base;
using Cinex.Core.Interfaces.Repositories;

namespace Cinex.Infrastructure.Data.DomainServices.Base
{
    public abstract class BaseDataService : IBaseDataService
    {
        private readonly IAuditTrailRepository _auditTrailRepository;

        public BaseDataService(IAuditTrailRepository auditTrailRepository)
        {
            _auditTrailRepository = auditTrailRepository;
        }
    }
}

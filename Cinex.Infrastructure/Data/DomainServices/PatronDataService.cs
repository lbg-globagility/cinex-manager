using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class PatronDataService : BaseSavableDataService<Patron>, IPatronDataService
    {
        private readonly IPatronRepository _patronRepository;

        public PatronDataService(
            IPatronRepository patronRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(
                patronRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _patronRepository = patronRepository;
        }

        public async Task<ICollection<Patron>> GetAllAsync() => await _patronRepository.GetAllAsync();

        protected override int ModuleCodeId(Patron entity = null)
        {
            throw new System.NotImplementedException();
        }

        protected override string TableName(Patron entity = null)
        {
            throw new System.NotImplementedException();
        }
    }
}

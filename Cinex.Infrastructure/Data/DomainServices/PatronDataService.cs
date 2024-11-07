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
            CinexContext context) :
            base(patronRepository, auditTrailRepository, context)
        {
            _patronRepository = patronRepository;
        }

        public async Task<ICollection<Patron>> GetAllAsync() => await _patronRepository.GetAllAsync();
    }
}

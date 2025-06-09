using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class EwalletDataService : BaseSavableDataService<Ewallet>, IEwalletDataService
    {
        private readonly IEwalletRepository _ewalletRepository;

        public EwalletDataService(
            IEwalletRepository ewalletRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(ewalletRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _ewalletRepository = ewalletRepository;
        }

        public async Task<ICollection<Ewallet>> GetAllAsync() => await _ewalletRepository.GetAllAsync();

        public async Task SetDefaultEwalletAsync(int userId, Ewallet eWallet)
        {
            var allEwallets = await GetAllAsync();
            foreach (var ewallet in allEwallets)
            {
                ewallet.IsDefault = eWallet.Id == ewallet.Id;
                ewallet.SetEdited();
            }

            if(allEwallets?.Any(t => t.IsDefault) ?? false) await SaveManyAsync(userId, updated: allEwallets.ToList());
        }

        protected override int ModuleCodeId(Ewallet entity = null)
        {
            if (entity?.IsNewEntity ?? false) return _systemModuleRepository.GetByCode(SystemModule.PATRON_ADD_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsEdited ?? false) return _systemModuleRepository.GetByCode(SystemModule.PATRON_EDIT_CODE_TEXT)?.Id ?? 0;
            if (entity?.IsDelete ?? false) return _systemModuleRepository.GetByCode(SystemModule.PATRON_DELETE_CODE_TEXT)?.Id ?? 0;
            return 0;
        }

        protected override string TableName(Ewallet entity = null) => Ewallet.TABLE_NAME;
    }
}

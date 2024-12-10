using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class ConfigurationDataService : BaseSavableDataService<Configuration>, IConfigurationDataService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationDataService(IConfigurationRepository configurationRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(configurationRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<ICollection<Configuration>> GetAllAsync() => await _configurationRepository.GetAllAsync();

        public async Task<Configuration> GetByDescriptionAsync(string description) => (await GetAllAsync())
            .FirstOrDefault(t => t.Description == description);

        public async Task<Configuration> GetORNumberFormatAsync() => await GetByDescriptionAsync(Configuration.DESCRIPTION_OR_NUMBER_FORMAT_TEXT);

        protected override int ModuleCodeId(Configuration entity = null)
        {
            throw new System.NotImplementedException();
        }

        protected override string TableName(Configuration entity = null) => Configuration.TABLE_NAME;
    }
}

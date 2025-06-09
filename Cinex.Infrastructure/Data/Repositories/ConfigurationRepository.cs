using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class ConfigurationRepository : SavableRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(CinexContext context) : base(context)
        {
        }
    }
}

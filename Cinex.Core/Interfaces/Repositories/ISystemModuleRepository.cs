using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories.Base;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.Repositories
{
    public interface ISystemModuleRepository : ISavableRepository<SystemModule>
    {
        Task<SystemModule> GetByCodeAsync(string code);

        SystemModule GetByCode(string code);
    }
}

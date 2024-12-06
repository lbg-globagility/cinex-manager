using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.DomainServices
{
    public interface ISessionEwalletDataService : IBaseSavableDataService<SessionEwallet>
    {
        Task<ICollection<SessionEwallet>> GetAllAsync();
    }
}

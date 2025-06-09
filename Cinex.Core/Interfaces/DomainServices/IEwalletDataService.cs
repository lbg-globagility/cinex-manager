using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.DomainServices
{
    public interface IEwalletDataService : IBaseSavableDataService<Ewallet>
    {
        Task<ICollection<Ewallet>> GetAllAsync();

        Task SetDefaultEwalletAsync(int userId, Ewallet eWallet);
    }
}

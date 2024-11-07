using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices.Base;
<<<<<<< Updated upstream
=======
using System.Collections.Generic;
using System.Threading.Tasks;
>>>>>>> Stashed changes

namespace Cinex.Core.Interfaces.DomainServices
{
    public interface ICinemaDataService : IBaseSavableDataService<Cinema>
    {
<<<<<<< Updated upstream
=======
        Task<ICollection<Cinema>> GetAllAsync();
>>>>>>> Stashed changes
    }
}

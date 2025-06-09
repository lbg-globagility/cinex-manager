using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.DomainServices
{
    public interface IMovieScheduleListReserveSeatDataService : IBaseSavableDataService<MovieScheduleListReserveSeat>
    {
        Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync();

        Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId);
    }
}

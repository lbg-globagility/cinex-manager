using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.Repositories
{
    public interface IMovieScheduleListReserveSeatRepository : ISavableRepository<MovieScheduleListReserveSeat>
    {
        Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId);
    }
}

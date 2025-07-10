using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.DomainServices
{
    public interface IMovieScheduleListReserveSeatDataService : IBaseSavableDataService<MovieScheduleListReserveSeat>
    {
        Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync();

        Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId);

        Task<ICollection<MovieScheduleListReserveSeat>> GetByDateRangeAsync(DateTime start, DateTime end);

        Task<ICollection<MovieScheduleListReserveSeat>> GetByCompositeParamsAsync(DateTime start,
            DateTime end,
            int[] cinemaIds = null,
            string[] usernames = null,
            string[] terminals = null,
            int[] patronIds = null);
    }
}

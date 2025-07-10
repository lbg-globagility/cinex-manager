using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.DomainServices.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices
{
    public class MovieScheduleListReserveSeatDataService : BaseSavableDataService<MovieScheduleListReserveSeat>, IMovieScheduleListReserveSeatDataService
    {
        private readonly IMovieScheduleListReserveSeatRepository _movieScheduleListReserveSeatRepository;

        public MovieScheduleListReserveSeatDataService(IMovieScheduleListReserveSeatRepository movieScheduleListReserveSeatRepository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(movieScheduleListReserveSeatRepository,
                auditTrailRepository,
                systemModuleRepository,
                context)
        {
            _movieScheduleListReserveSeatRepository = movieScheduleListReserveSeatRepository;
        }

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync() => await _movieScheduleListReserveSeatRepository.GetAllAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId) => await _movieScheduleListReserveSeatRepository.GetBySessionIdAsync(sessionId);

        protected override int ModuleCodeId(MovieScheduleListReserveSeat entity = null)
        {
            throw new System.NotImplementedException();
        }

        protected override string TableName(MovieScheduleListReserveSeat entity = null)
        {
            throw new System.NotImplementedException();
        }
        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByDateRangeAsync(DateTime start, DateTime end) => await _movieScheduleListReserveSeatRepository.GetByDateRangeAsync(start: start, end: end);

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByCompositeParamsAsync(DateTime start, DateTime end, int[] cinemaIds = null, string[] usernames = null, string[] terminals = null, int[] patronIds = null) => await _movieScheduleListReserveSeatRepository.GetByCompositeParamsAsync(start: start, end: end, cinemaIds: cinemaIds, usernames: usernames, terminals: terminals, patronIds: patronIds);
    }
}

using AutoMapper;
using Cinex.API.Models;
using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinex.API.Controllers
{
    [ApiController]
    [Route("api/local")]
    public class LocalController : ControllerBase
    {
        private readonly CinexContext _context;
        private readonly IReservationRepository _reservationRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogger<LocalController> _logger;
        private readonly IMapper _mapper;

        public LocalController(CinexContext context, IReservationRepository reservationRepository, ILogger<LocalController> logger, IMapper mapper, ISessionRepository sessionRepository)
        {
            _context = context;
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper= mapper;
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        public IActionResult Ping() => Ok();

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] BuyTicketModel model)
        {
            var createSession = _sessionRepository.NewSession(model.SessionID,model.Amount);
            var created = await _reservationRepository.CreateReservation(model);

            return created ? Ok() : Conflict("Seat has been taken");
        }

        [HttpPost("new-schedule-list")]
        public async Task<ActionResult<List<MovieScheduleDto>>> GetNewScheduleList([FromBody] List<DateTime> period)
        {
            var start = period[0];
            var end = period[1];

            var schedules = await _context.Set<MovieSchedule>()
                .Include(x=>x.MovieScheduleLists).AsNoTracking()
                .Where(s => s.Date >= start && s.Date <= end)
                .ToListAsync();
           var map =  _mapper.Map<List<MovieScheduleDto>>(schedules);
            return Ok(map);
        }

        [HttpGet("pull-movie")]
        public async Task<ActionResult<List<Movie>>> PullMovie()
        {
            var movies = await _context.Set<Movie>()
                .AsNoTracking()
                .ToListAsync();

            return Ok(movies);
        }

        // movies has no sync-state column, so this only acknowledges the ids rather than persisting anything.
        [HttpPost("sync-movie")]
        public IActionResult SyncMovie([FromBody] List<int> ids) => Ok(ids?.Count ?? 0);

        // movies_schedule has no sync-state column, so this only acknowledges the ids rather than persisting anything.
        [HttpPost("sync-movie-schedule")]
        public IActionResult SyncMovieSchedule([FromBody] List<int> ids) => Ok(ids?.Count ?? 0);

        [HttpGet("patrons")]
        public async Task<ActionResult<List<Patron>>> GetPatrons()
        {
            var patrons = await _context.Set<Patron>()
                .AsNoTracking()
                .ToListAsync();

            return Ok(patrons);
        }
        [HttpPost("check-available-seats")]
        public async Task<bool> CheckSeatAvailability([FromBody] SeatValidationModel model)
        {
            var available = await _reservationRepository.CheckIfTheSeatIsAvailable(model.MovieScheduleListID,model.CinemaSeatIDs);
            return available;
        }
    }
}

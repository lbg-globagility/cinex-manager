using Cinex.API.Models;
using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
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
        private readonly ILogger<LocalController> _logger;

        public LocalController(CinexContext context, IReservationRepository reservationRepository, ILogger<LocalController> logger)
        {
            _context = context;
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Ping() => Ok();

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] BuyTicketModel model)
        {
            var created = await _reservationRepository.CreateReservation(model);
            return created ? Ok() : Problem("Failed to create ticket.");
        }

        [HttpPost("new-schedule-list")]
        public async Task<ActionResult<List<MovieSchedule>>> GetNewScheduleList([FromBody] List<DateTime> period)
        {
            var start = period[0];
            var end = period[1];

            var schedules = await _context.Set<MovieSchedule>()
                .AsNoTracking()
                .Where(s => s.Date >= start && s.Date <= end)
                .ToListAsync();

            return Ok(schedules);
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
    }
}

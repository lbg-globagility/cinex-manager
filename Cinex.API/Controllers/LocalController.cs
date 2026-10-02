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
        private readonly IMovieScheduleRepository _movieScheduleRepository;
        private readonly ILogger<LocalController> _logger;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public LocalController(CinexContext context, 
            IReservationRepository reservationRepository,
            ILogger<LocalController> logger, IMapper mapper,
            ISessionRepository sessionRepository,
            IMovieScheduleRepository movieScheduleRepository,
            IMovieRepository movieRepository)
        {
            _context = context;
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
            _sessionRepository = sessionRepository;
            _movieScheduleRepository = movieScheduleRepository;
            _movieRepository = movieRepository;
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
        [HttpGet("movie-schedule/{movieCode}")]
        public async Task<List<MovieScheduleDto>> GetMovieSchedule(string movieCode)
        {
            var movie =await _movieRepository.MovieDetails(movieCode);
            if (movie == null)
            {
                throw new Exception("no moviue");
            }
            var schedules = await _movieScheduleRepository.GetSchedules(movie.Id);
            var today = schedules.Where(x => x.Date.Date >= DateTime.Now.Date).ToList();
            return today.Select(x => _mapper.Map<MovieScheduleDto>(x)).ToList();
        }
        [HttpGet("movie-schedule-list/{hash}/{id}")]
        public async Task<MovieScheduleListDto> GetMovieScheduleList(string hash, int id)
        {
            var scheduleList = await _movieScheduleRepository.GetScheduleListByHash(hash, id);
            return _mapper.Map<MovieScheduleListDto>(scheduleList);
        }


        [HttpPost("new-schedule-list")]
        public async Task<ActionResult<List<MovieScheduleDto>>> GetNewScheduleList([FromBody] List<DateTime> period)
        {
            var start = period[0];
            var end = period[1];
            
            var schedules = await _context.Set<MovieSchedule>()
                .Include(x=>x.MovieScheduleLists).ThenInclude(x=>x.MovieScheduleListPatrons).AsNoTracking()
                .Where(s => s.Date >= start && s.Date <= end && !s.is_sync)
                .ToListAsync();
           var map =  _mapper.Map<List<MovieScheduleDto>>(schedules);
            return Ok(map);
        }

        [HttpGet("pull-movie")]
        public async Task<ActionResult<List<Movie>>> PullMovie()
        {
            var movies = await _context.Set<Movie>()
                .AsNoTracking().Where(x=>!x.is_sync)
                .ToListAsync();

            return Ok(movies);
        }

        // movies has no sync-state column, so this only acknowledges the ids rather than persisting anything.
        [HttpPost("sync-movie")]
        public async Task<IActionResult> SyncMovie([FromBody] List<int> ids)
        {
            var movies = await _context.Set<Movie>().Where(x => ids.Contains(x.Id)).ToListAsync();

            // 2. Make your modifications
            foreach (var movie in movies)
            {
                movie.is_sync = true;
            }

            await _context.SaveChangesAsync();
            return Ok(ids?.Count ?? 0);
        }

        // movies_schedule has no sync-state column, so this only acknowledges the ids rather than persisting anything.
        [HttpPost("sync-movie-schedule")]
        public async Task<IActionResult> SyncMovieSchedule([FromBody] List<int> ids)
        {

            var movieschedules = await _context.Set<MovieSchedule>().Where(x => ids.Contains(x.Id)).ToListAsync();

            // 2. Make your modifications
            foreach (var schedule in movieschedules)
            {
                schedule.is_sync = true;
            }

            await _context.SaveChangesAsync();
            return Ok(ids?.Count ?? 0);
        }

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

using AutoMapper;
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

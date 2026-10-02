using AutoMapper;
using Cinex.API.Models;
using Cinex.Core.Entities;

namespace Cinex.API.AutomapperProfile
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration() : this("My Profile")
        {
        }

        private AutoMapperProfileConfiguration(string profileName) : base(profileName)
        {
            CreateMap<MovieSchedule, MovieScheduleDto>();
            CreateMap<MovieScheduleList, MovieScheduleDto.MovieScheduleListDto>();
            CreateMap<MovieScheduleListPatron, MovieScheduleDto.MovieScheduleListDto.MovieScheduleListPatronDto>();

            CreateMap<MovieScheduleList, MovieScheduleListDto>();
            CreateMap<MovieSchedule, MovieScheduleListDto.MovieScheduleDto>();
            CreateMap<MovieScheduleListPatron, MovieScheduleListDto.MovieScheduleListPatronDto>();
            CreateMap<Cinema, MovieScheduleListDto.MovieScheduleDto.CinemaDto>();
            CreateMap<CinemaSeat, MovieScheduleListDto.MovieScheduleDto.CinemaDto.CinemaSeatDto>();
        }
    }
}

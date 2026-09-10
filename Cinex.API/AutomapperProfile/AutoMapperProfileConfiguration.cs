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
            CreateMap<MovieScheduleList, MovieScheduleListDto>();
        }
    }
}

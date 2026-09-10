using AutoMapper;
using Cinex.API.AutomapperProfile;
using Microsoft.Extensions.DependencyInjection;
namespace Cinex.API.Startups
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}

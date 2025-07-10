using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data;
using Cinex.Infrastructure.Data.DomainServices;
using Cinex.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cinex.Test
{
    public abstract class DatabaseTest
    {
        public const string CONNECTION_STRING = "server=localhost; user id=root; password=Am3L74rS; database=azynema_gaisano_alltest2; port=3308; default command timeout=240;";

        public DatabaseTest()
        {
            ConfigureDependencyInjection();
        }

        public ServiceProvider MainServiceProvider { get; private set; }

        private void ConfigureDependencyInjection()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            MainServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<CinexContext>(options =>
            {
                options.UseMySql(connectionString: CONNECTION_STRING)//, serverVersion: ServerVersion.AutoDetect(CONNECTION_STRING)
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Repository
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<ICinemaPatronRepository, CinemaPatronRepository>();
            services.AddScoped<ICinemaPatronDefaultRepository, CinemaPatronDefaultRepository>();
            services.AddScoped<IEwalletRepository, EwalletRepository>();
            services.AddScoped<IPatronRepository, PatronRepository>();
            services.AddScoped<ISystemModuleRepository, SystemModuleRepository>();
            services.AddScoped<IMovieScheduleListReserveSeatRepository, MovieScheduleListReserveSeatRepository>();

            // Data Service
            services.AddScoped<ICinemaDataService, CinemaDataService>();
            services.AddScoped<ICinemaPatronDataService, CinemaPatronDataService>();
            services.AddScoped<ICinemaPatronDefaultDataService, CinemaPatronDefaultDataService>();
            services.AddScoped<IEwalletDataService, EwalletDataService>();
            services.AddScoped<IPatronDataService, PatronDataService>();
            services.AddScoped<IMovieScheduleListReserveSeatDataService, MovieScheduleListReserveSeatDataService>();
            
        }
    }
}

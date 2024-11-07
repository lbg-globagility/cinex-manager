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
        public const string CONNECTION_STRING = "server=localhost; user id=root; password=Am3L74rS; database=azynema_fsm; port=3309; default command timeout=240;";

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
<<<<<<< Updated upstream
                options.UseMySql(connectionString: CONNECTION_STRING, serverVersion: ServerVersion.AutoDetect(CONNECTION_STRING))
=======
                options.UseMySql(connectionString: CONNECTION_STRING)//, serverVersion: ServerVersion.AutoDetect(CONNECTION_STRING)
>>>>>>> Stashed changes
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Repository
            services.AddScoped<ICinemaRepository, CinemaRepository>();

            // Data Service
            services.AddScoped<ICinemaDataService, CinemaDataService>();
        }
    }
}

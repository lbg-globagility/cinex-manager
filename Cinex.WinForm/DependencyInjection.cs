using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data;
using Cinex.Infrastructure.Data.DomainServices;
using Cinex.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Cinex.WinForm
{
    public abstract class DependencyInjection
    {
        private readonly string _connectionString;

        public DependencyInjection(string connectionString = "password=Am3L74rS;server=127.0.0.1;user id=root;database=azynema_fsm;port=3309;")
        {
            _connectionString = connectionString;

            var services = new ServiceCollection();

            ConfigureServices(services);

            MainServiceProvider = services.BuildServiceProvider();
        }

        public readonly ServiceProvider MainServiceProvider;

        //private void ConfigureDependencyInjection()
        //{
        //    var services = new ServiceCollection();

        //    ConfigureServices(services);

        //    _MainServiceProvider = services.BuildServiceProvider();
        //}

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<CinexContext>(options =>
            {
                options.UseMySql(connectionString: _connectionString,
                    o =>
                    {
                        o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(24), null);
                    })//, serverVersion: ServerVersion.AutoDetect(_connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
                //.UseQueryTrackingBehavior(default);
            });

            // Repository
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<ICinemaPatronRepository, CinemaPatronRepository>();
            services.AddScoped<ICinemaPatronDefaultRepository, CinemaPatronDefaultRepository>();
            services.AddScoped<IEwalletRepository, EwalletRepository>();
            services.AddScoped<IPatronRepository, PatronRepository>();
            services.AddScoped<ISessionEwalletRepository, SessionEwalletRepository>();
            services.AddScoped<ISystemModuleRepository, SystemModuleRepository>();

            // Data Service
            services.AddScoped<ICinemaDataService, CinemaDataService>();
            services.AddScoped<ICinemaPatronDataService, CinemaPatronDataService>();
            services.AddScoped<ICinemaPatronDefaultDataService, CinemaPatronDefaultDataService>();
            services.AddScoped<IEwalletDataService, EwalletDataService>();
            services.AddScoped<IPatronDataService, PatronDataService>();
            services.AddScoped<ISessionEwalletDataService, SessionEwalletDataService>();
        }
    }
}

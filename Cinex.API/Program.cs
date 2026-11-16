using Cinex.API.Services;
using Cinex.API.Services.Interfaces;
using Cinex.API.Startups;
using Cinex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinex.API", Version = "v1" }));

            services.AddDbContext<CinexContext>(options =>
            {
                options.UseMySql(
                    context.Configuration.GetConnectionString("CinexDb"),
                    o => o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(24), null));
            });

            services.AddDbContext<OnlineCinexContext>(options =>
            {
                options.UseMySql(
                    context.Configuration.GetConnectionString("Online"),
                    o => o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(24), null));
            });

            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            //services.AddHostedService<ReserveSeatSyncService>();

            services.AddAutoMapper();
        });

        webBuilder.Configure((context, app) =>
        {
            // Configure the HTTP request pipeline.
            if (context.HostingEnvironment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cinex.API v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        });
    })
    .Build()
    .Run();

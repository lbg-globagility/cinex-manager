using Cinex.API.Services;
using Cinex.API.Services.Interfaces;
using Cinex.API.Startups;
using Cinex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CinexContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("CinexDb"),
        o => o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(24), null));
});

builder.Services.AddDbContext<OnlineCinexContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("Online"),
        o => o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(24), null));
});

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
//builder.Services.AddHostedService<ReserveSeatSyncService>();

builder.Services.AddAutoMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

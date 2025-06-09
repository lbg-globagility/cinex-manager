using Cinex.Core.Interfaces.DomainServices;
using Cinex.WinForm;
using CommonLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Paradiso.EF
{
    internal class DependencyInjectionHelper : DependencyInjection
    {
        public static string CONNECTION_STRING = CommonUtility.ConnectionString;

        private readonly ServiceProvider _mainServiceProvider;

        public DependencyInjectionHelper() : base(connectionString: CONNECTION_STRING)
        {
            _mainServiceProvider = MainServiceProvider;
        }

        public static ICinemaDataService GetCinemaDataService => NewInstance.MainServiceProvider.GetRequiredService<ICinemaDataService>();

        public static ICinemaPatronDataService GetCinemaPatronDataService => NewInstance.MainServiceProvider.GetRequiredService<ICinemaPatronDataService>();

        public static ICinemaPatronDefaultDataService GetCinemaPatronDefaultDataService => NewInstance.MainServiceProvider.GetRequiredService<ICinemaPatronDefaultDataService>();

        public static IPatronDataService GetPatronDataService => NewInstance.MainServiceProvider.GetRequiredService<IPatronDataService>();

        public static IEwalletDataService GetEwalletDataService => NewInstance.MainServiceProvider.GetRequiredService<IEwalletDataService>();

        public static ISessionEwalletDataService GetSessionEwalletDataService => NewInstance.MainServiceProvider.GetRequiredService<ISessionEwalletDataService>();

        public static IMovieScheduleListReserveSeatDataService GetMovieScheduleListReserveSeatDataService => NewInstance.MainServiceProvider.GetRequiredService<IMovieScheduleListReserveSeatDataService>();
        
        public static DependencyInjectionHelper NewInstance => new DependencyInjectionHelper();
    }
}

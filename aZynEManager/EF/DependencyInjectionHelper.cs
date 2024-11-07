using Cinex.Core.Interfaces.DomainServices;
using Cinex.WinForm;
using CommonLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aZynEManager.EF
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

        public static IPatronDataService GetPatronDataService => NewInstance.MainServiceProvider.GetRequiredService<IPatronDataService>();

        public static DependencyInjectionHelper NewInstance => new DependencyInjectionHelper();
    }
}

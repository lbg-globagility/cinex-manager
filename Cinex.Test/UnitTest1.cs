using Cinex.Core.Entities;
using Cinex.Core.Interfaces.DomainServices;
using Cinex.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cinex.Test
{
    public class Tests : DatabaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                var cinemaRepository = MainServiceProvider.GetRequiredService<ICinemaRepository>();

                var cinema = await cinemaRepository.GetByIdAsync(id: 10);

                //cinema.Capacity = 182;
                cinema.Capacity += 1;
                cinema.SetEdited();

                var cinemaDataService = MainServiceProvider.GetRequiredService<ICinemaDataService>();
                await cinemaDataService.SaveManyAsync(userId: 1, updated: new List<Cinema> { cinema });

                Assert.NotNull(cinema);
            }
            catch (Exception ex)
            {
                throw GetInnerException(ex);
            }
        }

        private Exception GetInnerException(Exception ex)
        {
            if (ex?.InnerException != null) return GetInnerException(ex.InnerException);

            return ex;
        }

        [Test]
        public async Task Test2()
        {
            try
            {
                var eWalletDataService = MainServiceProvider.GetRequiredService<IEwalletDataService>();
                var eWallet = await eWalletDataService.GetByIdAsync(1);
                await eWalletDataService.SetDefaultEwalletAsync(1, eWallet)
                    .ContinueWith(async (a) => {
                        if (!a.IsCompleted) return;

                        eWallet = await eWalletDataService.GetByIdAsync(2);
                        await eWalletDataService.SetDefaultEwalletAsync(1, eWallet);

                    }, TaskScheduler.Default);

                
            }
            catch (Exception ex)
            {
                throw GetInnerException(ex);
            }
        }

        [Test]
        public async Task Test3()
        {
            try
            {
                var movieScheduleListReserveSeatDataService = MainServiceProvider.GetRequiredService<IMovieScheduleListReserveSeatDataService>();
                var movieScheduleListReserveSeats = await movieScheduleListReserveSeatDataService.GetBySessionIdAsync("20241101-144000-239");
                //.ContinueWith((a) => {
                //    if (!a.IsCompleted) return;


                //}, TaskScheduler.Default);

                Console.WriteLine(movieScheduleListReserveSeats?.Count() ?? 0);
            }
            catch (Exception ex)
            {
                throw GetInnerException(ex);
            }
        }


        [Test]
        public async Task Test4()
        {
            try
            {
                var movieScheduleListReserveSeatDataService = MainServiceProvider.GetRequiredService<IMovieScheduleListReserveSeatDataService>();
                //var movieScheduleListReserveSeats = await movieScheduleListReserveSeatDataService.GetByDateRangeAsync(start: new DateTime(2025, 6, 6),
                //    end: new DateTime(2025, 6, 6));
                var movieScheduleListReserveSeats = await movieScheduleListReserveSeatDataService
                    .GetByCompositeParamsAsync(start: new DateTime(2025, 7, 1),
                        end: new DateTime(2025, 7, 1),
                        cinemaIds: new int[] { 12, 13, 14, 15, 17, 18, 19, 20 },
                        usernames: new string[] { "ADMIN", "ADMIN", "ATALUNA", "JPEPITO", "ATANGAG", "RNUDALO", "MJARO", "JCANASA", "DALILIN", "ESUMILHIG", "JGABATO", "JLAYUPAN", "RCOMENDADOR", "RMONTES", "GLOBAGILITY", "GLOBAGILITY", "DALILIN", "JGABATO", "ESUMILHIG", "RMONTES", "RCOMENDADOR"},
                        terminals: new string[] { "DVO-CNM-TCK-01", "LAMBRRRT", "DVO-CNM-TCK-02", "DVO-CNM-TCK-03", "DVO-CNM-TCK-04" },
                        patronIds: new int[] { 24, 19, 26, 23, 17, 16, 18, 21, 22 });

                var count = movieScheduleListReserveSeats?.Count() ?? 0;

                Console.WriteLine(count);
            }
            catch (Exception ex)
            {
                throw GetInnerException(ex);
            }
        }
    }
}

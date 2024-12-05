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
    }
}
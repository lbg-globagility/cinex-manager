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
<<<<<<< Updated upstream
            Assert.True(true);

=======
>>>>>>> Stashed changes
            var cinemaRepository = MainServiceProvider.GetRequiredService<ICinemaRepository>();

            var cinema = await cinemaRepository.GetByIdAsync(id: 10);

            Assert.NotNull(cinema);
        }
    }
}
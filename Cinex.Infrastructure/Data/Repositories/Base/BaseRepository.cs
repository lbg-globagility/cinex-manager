
namespace Cinex.Infrastructure.Data.Repositories.Base
{
    public abstract class BaseRepository
    {
        public bool IsNewEntity(int id) => id <= 0;
    }
}

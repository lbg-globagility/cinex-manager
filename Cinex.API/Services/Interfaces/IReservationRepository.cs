using Cinex.API.Models;
using System.Threading.Tasks;

namespace Cinex.API.Services.Interfaces
{
    public interface IReservationRepository
    {
        Task<bool> CreateReservation(BuyTicketModel model);
    }
}

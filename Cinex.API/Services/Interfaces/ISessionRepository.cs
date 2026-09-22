namespace Cinex.API.Services.Interfaces
{
    public interface ISessionRepository
    {
        Task NewSession(string sessionId, decimal amount);
    }
}

using Domain.Entities;

namespace Application.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<bool> CheckEmailExist(string email);
        Task<Player> GetPlayerByEmail(string email);
        Task<Player> Verify(string token);
    }
}

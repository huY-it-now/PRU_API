using Domain.Entities;

namespace Application.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<bool> CheckEmailExist(string email);
    }
}

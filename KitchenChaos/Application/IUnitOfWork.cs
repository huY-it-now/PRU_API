using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IPlayerRepository playerRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}

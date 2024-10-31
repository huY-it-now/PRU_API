using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IPlayerRepository _playerRepository;

        public UnitOfWork(AppDbContext dbContext, IPlayerRepository playerRepository)
        {
            _dbContext = dbContext;
            _playerRepository = playerRepository;
        }

        public IPlayerRepository playerRepository => _playerRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

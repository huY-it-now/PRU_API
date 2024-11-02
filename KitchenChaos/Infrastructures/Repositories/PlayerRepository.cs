using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly AppDbContext _dbContext;

        public PlayerRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _dbContext.Players.AnyAsync(e => e.Email == email);
        }

        public async Task<Player> GetPlayerByEmail(string email)
        {
            return await _dbContext.Players.Include(x => x.Records).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Player> Verify(string token)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(t => t.VerificationToken == token);
        }
    }
}

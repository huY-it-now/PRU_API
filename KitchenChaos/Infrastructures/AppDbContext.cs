using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Data;

namespace Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .HasOne(x => x.Player)
                .WithMany(x => x.Records)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Player_Record");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RateItBot.Domain.Entities;

namespace RateItBot.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Rating)
                .WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId); ;
        }
    }
}

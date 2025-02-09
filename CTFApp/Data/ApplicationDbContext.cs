using CTFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    userName = "admin",
                    userScore = "1200000"
                },
                new User
                {
                    Id = "2",
                    userName = "Minh",
                    userScore = "4000"
                }
            );
        }

    }
}

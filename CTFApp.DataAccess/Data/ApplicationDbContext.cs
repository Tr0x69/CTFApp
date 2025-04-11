using CTFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Flag> Flag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("tr0x69"),
                userScore = 0,
                Role = "Admin"
            });

            modelBuilder.Entity<Flag>().HasData(
                new Flag
                {
                    Id = 1,
                    flag = "CTFApp{example_flag_content}"
                }
            );


        }


    }
}

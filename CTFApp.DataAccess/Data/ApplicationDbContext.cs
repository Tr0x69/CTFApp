using Microsoft.EntityFrameworkCore;
using CTFApp.Models;

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

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    userName = "admin",
                    userScore = 1200000
                },
                new User
                {
                    Id = "2",
                    userName = "Minh",
                    userScore = 4000
                }
            );


            modelBuilder.Entity<Flag>().HasData(
                new Flag
                {
                    Id = 1,
                    flag = "ctf{example_flag_content}"
                }
            );


        }


    }
}

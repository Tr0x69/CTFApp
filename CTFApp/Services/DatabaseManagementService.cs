using CTFApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Services
{
    public class DatabaseManagementService
    {
        //check if there is a database exist or not. If not, create a new database


        //important as you must have to apply the migrations to the database before it start in docker. The web application will not start nor create exist if you do not apply the migrations to the database.
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();

            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Enums;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using static System.Collections.Specialized.BitVector32;

namespace PrimeCareMed.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {

        string path = @"Seed";
        List<Medicine> medicines = new List<Medicine>();
        if (!roleManager.Roles.Any())
        {
            foreach(var name in Enum.GetNames(typeof(UserRole))) 
            {
                await roleManager.CreateAsync(new IdentityRole(name));
            }
        }
        if (!userManager.Users.Any())
        {
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true, FirstName="admin", LastName="admin"};
            var user1 = new ApplicationUser { UserName = "sysadmin@admin.com", Email = "sysadmin@admin.com", EmailConfirmed = true, FirstName = "sys", LastName = "admin" };

            await userManager.CreateAsync(user, "Admin123.?");
            await userManager.AddToRoleAsync(user, "Administrator");

            await userManager.CreateAsync(user1, "SysAdmin123.?");
            await userManager.AddToRoleAsync(user1, "SysAdministrator");
        }
        //if (!context.Medicines.Any())
        //{

        //    //var medicinesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "stations.json");
        //    //stations = JsonConvert.DeserializeObject<List<Station>>(stationsJson);
        //    //await context.Stations.AddRangeAsync(stations);
        //    //await context.SaveChangesAsync();

        //}
    }
}

using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Enums;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json;

namespace PrimeCareMed.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {

        string path = @"Seed";
        List<Medicine> medicines = new List<Medicine>();
        List<Vaccine> vaccines = new List<Vaccine>();
        List<GeneralMedicineOffice> offices = new List<GeneralMedicineOffice>();
        if (!roleManager.Roles.Any())
        {
            foreach (var name in Enum.GetNames(typeof(UserRole)))
            {
                await roleManager.CreateAsync(new IdentityRole(name));
            }
        }
        if (!userManager.Users.Any())
        {
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true, FirstName = "admin", LastName = "admin" };
            var user1 = new ApplicationUser { UserName = "sysadmin@admin.com", Email = "sysadmin@admin.com", EmailConfirmed = true, FirstName = "sys", LastName = "admin" };

            await userManager.CreateAsync(user, "Admin123.?");
            await userManager.AddToRoleAsync(user, "Administrator");

            await userManager.CreateAsync(user1, "Admin123.?");
            await userManager.AddToRoleAsync(user1, "SysAdministrator");
        }
        if (!context.Medicines.Any())
        {

            var medicinesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "medicines.json");
            medicines = JsonConvert.DeserializeObject<List<Medicine>>(medicinesJson);
            await context.Medicines.AddRangeAsync(medicines);
            await context.SaveChangesAsync();

        }
        if (!context.Vaccines.Any())
        {

            var vaccinesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "vaccines.json");
            vaccines = JsonConvert.DeserializeObject<List<Vaccine>>(vaccinesJson);
            await context.Vaccines.AddRangeAsync(vaccines);
            await context.SaveChangesAsync();
        }
        if (!context.GeneralMedicineOffices.Any())
        {
            var officesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "offices.json");
            offices = JsonConvert.DeserializeObject<List<GeneralMedicineOffice>>(officesJson);
            await context.GeneralMedicineOffices.AddRangeAsync(offices);
            await context.SaveChangesAsync();
        }
    }
}

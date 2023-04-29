using Microsoft.AspNetCore.Identity;
using BookIt.Core.Enums;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Entities;

namespace BookIt.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
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

            await userManager.CreateAsync(user, "Admin123.?");
            await userManager.AddToRoleAsync(user, "Administrator");

        }

        await context.SaveChangesAsync();
    }
}

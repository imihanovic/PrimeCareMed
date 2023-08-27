using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Enums;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using Bogus;
using Newtonsoft.Json;

namespace PrimeCareMed.DataAccess.Persistence;

public static class DatabaseContextSeed
{

    public static List<Patient> PatientsSeed = PatientInit(10);
    public static List<ApplicationUser> UserSeed = UserInit(10);
    public static async Task SeedDatabaseAsync(DatabaseContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        string path = @"Seed";
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

            await userManager.CreateAsync(user, "Admin1!");
            await userManager.AddToRoleAsync(user, "Administrator");

            await userManager.CreateAsync(user1, "Admin1!");
            await userManager.AddToRoleAsync(user1, "SysAdministrator");

            var roles = new List<string> { "Doctor", "Nurse"};
            foreach (var userFromSeed in UserSeed)
            {
                var random = new Random();
                userFromSeed.UserName = userFromSeed.Email;
                await userManager.CreateAsync(userFromSeed, "Admin1!");
                await userManager.AddToRoleAsync(userFromSeed, roles[random.Next(roles.Count)]);
            }
        }
        if (!context.Patients.Any())
        {
            await context.Patients.AddRangeAsync(PatientsSeed);
            await context.SaveChangesAsync();
        }
        if (!context.Medicines.Any())
        {
            var medicinesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "medicines.json");
            var medicines = JsonConvert.DeserializeObject<List<Medicine>>(medicinesJson);
            await context.Medicines.AddRangeAsync(medicines);
            await context.SaveChangesAsync();
        }
        if (!context.Vaccines.Any())
        {
            var vaccinesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "vaccines.json");
            var vaccines = JsonConvert.DeserializeObject<List<Vaccine>>(vaccinesJson);
            await context.Vaccines.AddRangeAsync(vaccines);
            await context.SaveChangesAsync();
        }
        if (!context.GeneralMedicineOffices.Any())
        {
            var officesJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "offices.json");
            var offices = JsonConvert.DeserializeObject<List<GeneralMedicineOffice>>(officesJson);
            await context.GeneralMedicineOffices.AddRangeAsync(offices);
            await context.SaveChangesAsync();
        }
    }
    
    public static List<Patient> PatientInit(int count)
    {
        var patientFaker = new Faker<Patient>()
           .RuleFor(p => p.FirstName, f => f.Person.FirstName)
           .RuleFor(p => p.LastName, f => f.Person.LastName)
           .RuleFor(p=>p.DateOfBirth, f=>f.Person.DateOfBirth.Date.ToUniversalTime())
           .RuleFor(p=>p.Email, f=>f.Person.Email)
           .RuleFor(p=>p.PhoneNumber, f=>f.Person.Phone)
           .RuleFor(p=>p.Oib, f=>string.Join("", f.Random.Digits(11)))
           .RuleFor(p=>p.Mbo, f => string.Join("", f.Random.Digits(9)))
           .RuleFor(p=>p.Gender, f=>f.PickRandom<Gender>());

        return patientFaker.Generate(count);

    }
    public static List<ApplicationUser> UserInit(int count)
    {
        var userFaker = new Faker<ApplicationUser>()
           .RuleFor(p => p.FirstName, f => f.Person.FirstName)
           .RuleFor(p => p.LastName, f => f.Person.LastName)
           .RuleFor(p => p.Email, f => f.Person.Email)
           .RuleFor(p => p.EmailConfirmed, true);
        return userFaker.Generate(count);
    }
}

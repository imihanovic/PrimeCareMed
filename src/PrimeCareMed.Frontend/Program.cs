using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.DataAccess.Persistence;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Shared.Services;
using PrimeCareMed.Shared.Services.Impl;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.Frontend.Policies;
using PrimeCareMed.DataAccess.Repositories.Impl;
using PrimeCareMed.Application.Common.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireAdministratorRoleOrAnonymous",
         policy => policy.AddRequirements(new AllowAnonymousAuthorizationRequirement()));
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<SmtpSettings>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IVaccineService, VaccineService>();
builder.Services.AddScoped<IVaccineRepository, VaccineRepository>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicinePrescriptionRepository, MedicinePrescriptionRepository>();
builder.Services.AddScoped<IMedicinePrescriptionService, MedicinePrescriptionService>();
builder.Services.AddScoped<IPatientVaccineRepository, PatientVaccineRepository>();
builder.Services.AddScoped<IPatientVaccineService, PatientVaccineService>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

//app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

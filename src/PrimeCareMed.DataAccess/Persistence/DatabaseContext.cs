﻿using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Shared.Services;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.DataAccess.Persistence;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    private readonly IClaimService _claimService;

    public DatabaseContext(DbContextOptions options, IClaimService claimService) : base(options)
    {
        _claimService = claimService;
    }

    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<GeneralMedicineOffice> GeneralMedicineOffices { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Shift> Shift { get; set; }
    public DbSet<PatientsVaccine> PatientsVaccines { get; set; }
    public DbSet<MedicinePrescription> MedicinePrescription { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
    public DbSet<Hospital> Hospital { get; set; }
    public DbSet<Checkup> Checkup { get; set; }
    public DbSet<HospitalCheckup> HospitalCheckup { get; set; }
    public DbSet<CheckupAppointment> CheckupAppointment { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        builder.Entity<Patient>()
        .HasIndex(u => u.Oib)
        .IsUnique();

        builder.Entity<Patient>()
       .HasIndex(u => u.Mbo)
       .IsUnique();

        builder.Entity<ApplicationUser>()
        .HasMany(r=>r.Patients)
        .WithOne(r=>r.Doctor)
        .IsRequired(false);

        builder.Entity<Shift>()
        .HasOne(e => e.Nurse)
        .WithMany(e => e.NursesShifts)
        .IsRequired();

        builder.Entity<Shift>()
        .HasOne(e => e.Doctor)
        .WithMany(e => e.DoctorsShifts)
        .IsRequired();

        builder.Entity<GeneralMedicineOffice>()
        .HasMany(e => e.Shifts)
        .WithOne(e => e.Office)
        .IsRequired();

        builder.Entity<Shift>()
        .HasMany(e => e.Appointments)
        .WithOne(e => e.Shift)
        .IsRequired();

        builder.Entity<HospitalCheckup>().HasKey(gu => new { gu.HospitalId, gu.CheckupId });

        builder.Entity<HospitalCheckup>().HasOne(ub => ub.Hospital)
               .WithMany(x => x.HospitalCheckups).HasForeignKey(h => h.HospitalId);

        builder.Entity<HospitalCheckup>().HasOne(ub => ub.Checkup)
            .WithMany(x => x.HospitalCheckups).HasForeignKey(h => h.CheckupId);


    }

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _claimService.GetUserId();
                    entry.Entity.CreatedOn = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _claimService.GetUserId();
                    entry.Entity.UpdatedOn = DateTime.Now;
                    break;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

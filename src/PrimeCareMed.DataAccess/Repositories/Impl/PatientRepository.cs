using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PatientRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients.OrderBy(r => r.LastName).ToListAsync();
        }
        public async Task<Patient> AddAsync(Patient patient)
        {
            Console.WriteLine($"DATUM REPO {patient.DateOfBirth}");
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Patients.FirstOrDefault(r => r.Id == id);
            _context.Patients.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Patient> UpdateAsync(Patient patient)
        {
            var editItem = await GetPatientByIdAsync(patient.Id);
            editItem.FirstName = patient.FirstName;
            editItem.LastName = patient.LastName;
            editItem.DateOfBirth= patient.DateOfBirth;
            editItem.Email = patient.Email;
            editItem.PhoneNumber = patient.PhoneNumber;
            editItem.Oib = patient.Oib;
            editItem.Mbo = patient.Mbo;
            editItem.Gender = patient.Gender;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            return await _context.Patients.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

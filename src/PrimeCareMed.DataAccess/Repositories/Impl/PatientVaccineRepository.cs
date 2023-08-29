using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class PatientVaccineRepository : IPatientVaccineRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PatientVaccineRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<PatientsVaccine>> GetAllPatientsVaccinesForAppointmentAsync(Guid id)
        {
            return await _context.PatientsVaccines.OrderByDescending(r => r.VaccineDate).Include(r=>r.Vaccine).Where(r=>r.Appointment.Id==id).ToListAsync();
        }
        public async Task<IEnumerable<PatientsVaccine>> GetAllPatientsVaccinesForPatientAsync(Guid id)
        {
            return await _context.PatientsVaccines.OrderByDescending(r => r.VaccineDate).Include(r => r.Vaccine).Include(r=>r.Appointment).ThenInclude(r=>r.Patient).Where(r => r.Appointment.Patient.Id == id).ToListAsync();
        }
        public bool CheckPatientsVaccinesForAppointmentAsync(Guid id)
        {
            var a = _context.PatientsVaccines.OrderByDescending(r => r.VaccineDate).Include(r => r.Vaccine).Where(r => r.Appointment.Id == id).FirstOrDefault();
            return a != null ? true : false;
        }
        public async Task<PatientsVaccine> AddAsync(PatientsVaccine patientsVaccine)
        {
            await _context.PatientsVaccines.AddAsync(patientsVaccine);
            await _context.SaveChangesAsync();
            return patientsVaccine;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.PatientsVaccines.FirstOrDefault(r => r.Id == id);
            _context.PatientsVaccines.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<PatientsVaccine> UpdateAsync(PatientsVaccine patientsVaccine)
        {
            var editItem = await GetPatientsVaccineByIdAsync(patientsVaccine.Id);
            editItem.Vaccine = patientsVaccine.Vaccine;
            editItem.VaccineDate = DateTime.Now.ToUniversalTime();
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<PatientsVaccine> GetPatientsVaccineByIdAsync(Guid id)
        {
            return await _context.PatientsVaccines.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

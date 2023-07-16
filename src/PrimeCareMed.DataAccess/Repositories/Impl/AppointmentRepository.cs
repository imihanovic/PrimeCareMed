using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AppointmentRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public Task<IEnumerable<Appointment>> GetAllAppointmentsForOfficeAsync(string Id)
        {
            var appointments = GetAllAppointmentsAsync();
            return (Task<IEnumerable<Appointment>>)appointments.Result.Where(r => r.Session.Shift.Office.Id.ToString() == Id);
        }
        public Task<IEnumerable<Appointment>> GetAllAppointmentsForDoctorAsync(string Id)
        {
            var appointments = GetAllAppointmentsAsync();
            return (Task<IEnumerable<Appointment>>)appointments.Result.Where(r => r.Session.Shift.Doctor.Id == Id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointment.OrderBy(r => r.Id).Include(r => r.Session).ThenInclude(r => r.Shift).ThenInclude(r => r.Office).Include(r => r.Session).ThenInclude(r => r.Shift).ThenInclude(r => r.Doctor).Include(r => r.Patient).ToListAsync();
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointment.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Appointment.FirstOrDefault(r => r.Id == id);
            _context.Appointment.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            var editItem = await GetAppointmentByIdAsync(appointment.Id);
            editItem.Status = appointment.Status;
            editItem.MedicalReport = appointment.MedicalReport;
            editItem.PatientsVaccines = appointment.PatientsVaccines;
            editItem.MedicinePrescriptions = appointment.MedicinePrescriptions;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointment.Include(r=>r.Session.Shift).Include(r=>r.Patient).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DatabaseContext _context;
        public AppointmentRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IEnumerable<Appointment> GetAllAppointmentsForOfficeAsync(string Id)
        {
            var appointments = GetAllAppointmentsAsync().Result.Where(r => r.Shift.Office.Id.ToString() == Id);
            return appointments;
        }
        public IEnumerable<Appointment> GetAllAppointmentsForPatientAsync(string Id)
        {
            var appointments = GetAllAppointmentsAsync().Result.Where(r => r.Patient.Id == Guid.Parse(Id)).OrderByDescending(r=>r.AppointmentDate);
            return appointments;
        }
        public IEnumerable<Appointment> GetAllAppointmentsForDoctorAsync(string Id)
        {
            var appointments = GetAllAppointmentsAsync().Result;
            return appointments.Where(r => r.Shift.Doctor.Id == Id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointment.OrderBy(r => r.Id).Include(r => r.Shift).ThenInclude(r => r.Office).Include(r => r.Shift).ThenInclude(r => r.Doctor).Include(r => r.Patient).ToListAsync();
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
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Appointment> FinishAppointmentAsync(Appointment appointment)
        {
            var editItem = GetAppointmentByIdAsync(appointment.Id).Result;
            editItem.Status = AppointmentStatus.Done;
            await _context.SaveChangesAsync();
            return editItem;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointment.Include(r=>r.Shift).Include(r=>r.Patient).ThenInclude(r=>r.Doctor).Include(r=>r.PatientsVaccines).ThenInclude(r=>r.Vaccine).Include(r=>r.MedicinePrescriptions).ThenInclude(r=>r.Medicine).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

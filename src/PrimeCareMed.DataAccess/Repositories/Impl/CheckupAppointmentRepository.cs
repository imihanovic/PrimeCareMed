using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class CheckupAppointmentRepository : ICheckupAppointmentRepository
    {
        private readonly DatabaseContext _context;
        public CheckupAppointmentRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<CheckupAppointment>> GetAllCheckupAppointmentsAsync(Guid CheckupId, Guid HospitalId)
        {
            return await _context.CheckupAppointment.OrderBy(r => r.Id).Include(r=>r.HospitalCheckup).ThenInclude(r=>r.Hospital).Include(r=>r.HospitalCheckup).ThenInclude(r=>r.Checkup).Where(r=>r.HospitalCheckup.CheckupId==CheckupId && r.HospitalCheckup.HospitalId == HospitalId).ToListAsync();
        }
        public async Task<CheckupAppointment> AddAsync(CheckupAppointment checkupAppointment)
        {
            await _context.CheckupAppointment.AddAsync(checkupAppointment);
            await _context.SaveChangesAsync();
            return checkupAppointment;
        }
        public async Task<IEnumerable<CheckupAppointment>> GetAllCheckupAppointmentsForPatientAsync(Guid PatientId)
        {
            return await _context.CheckupAppointment.OrderByDescending(r => r.CheckupDate).Include(r => r.HospitalCheckup).ThenInclude(r => r.Hospital).Include(r => r.HospitalCheckup).ThenInclude(r => r.Checkup).Include(r=>r.Appointment).ThenInclude(r=>r.Patient).Where(r => r.Appointment.Patient.Id == PatientId).ToListAsync();
        }
        public async Task<HospitalCheckup> AddHospitalCheckupAsync(HospitalCheckup hospitalCheckup)
        {
            await _context.HospitalCheckup.AddAsync(hospitalCheckup);
            await _context.SaveChangesAsync();
            return hospitalCheckup;
        }
        public async Task DeleteCheckupAppointmentAsync(Guid id)
        {
            var deleteItem = _context.CheckupAppointment.FirstOrDefault(r => r.Id == id);
            _context.CheckupAppointment.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<CheckupAppointment> UpdateAsync(CheckupAppointment checkupAppointment)
        {
            var editItem = await GetCheckupAppointmentByIdAsync(checkupAppointment.Id.ToString());
            editItem.CheckupStatus = checkupAppointment.CheckupStatus;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<CheckupAppointment> GetCheckupAppointmentByIdAsync(string id)
        {
            return await _context.CheckupAppointment.Include(r=>r.Appointment).ThenInclude(r => r.Patient).Include(r=>r.HospitalCheckup).ThenInclude(r=>r.Checkup).Include(r => r.HospitalCheckup).ThenInclude(r => r.Hospital).FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
    }
}

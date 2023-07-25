using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class MedicinePrescriptionRepository : IMedicinePrescriptionRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MedicinePrescriptionRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<MedicinePrescription>> GetAllMedicalPrecriptionsForAppointmentAsync(Guid Id)
        {
            return await _context.MedicinePrescription.OrderByDescending(r=>r.DatePrescribed).Include(r => r.Medicine).Where(r => r.Appointment.Id == Id).ToListAsync();
        }
        public async Task<MedicinePrescription> AddAsync(MedicinePrescription prescription)
        {
            await _context.MedicinePrescription.AddAsync(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }
        public bool CheckMedicinePrescriptionForAppointmentAsync(Guid id)
        {
            var a = _context.MedicinePrescription.OrderByDescending(r => r.DatePrescribed).Include(r => r.Medicine).Where(r => r.Appointment.Id == id).FirstOrDefault();
            return a != null ? true : false;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.MedicinePrescription.FirstOrDefault(r => r.Id == id);
            _context.MedicinePrescription.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<MedicinePrescription> UpdateAsync(MedicinePrescription prescription)
        {
            var editItem = await GetMedicalReportByIdAsync(prescription.Id);
            editItem.Medicine = prescription.Medicine;
            editItem.DatePrescribed = DateTime.Now.ToUniversalTime();
            editItem.Appointment = prescription.Appointment;
            editItem.Description = prescription.Description;
            editItem.IsActive = true;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<MedicinePrescription> GetMedicalReportByIdAsync(Guid id)
        {
            return await _context.MedicinePrescription.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

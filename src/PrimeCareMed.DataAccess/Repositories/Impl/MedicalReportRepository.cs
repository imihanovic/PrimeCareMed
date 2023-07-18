using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class MedicalReportRepository : IMedicalReportRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MedicalReportRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<MedicalReport>> GetAllMedicalReportsAsync()
        {
            return await _context.MedicalReports.OrderBy(r => r.ReportDate).ToListAsync();
        }
        public async Task<MedicalReport> AddAsync(MedicalReport report)
        {
            await _context.MedicalReports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.MedicalReports.FirstOrDefault(r => r.Id == id);
            _context.MedicalReports.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<MedicalReport> UpdateAsync(MedicalReport report)
        {
            var editItem = await GetMedicalReportByIdAsync(report.Id);
            editItem.Description = report.Description;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<MedicalReport> GetMedicalReportByIdAsync(Guid id)
        {
            return await _context.MedicalReports.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<MedicalReport> GetReportByAppointmentIdAsync(Guid id)
        {
            var a = await _context.MedicalReports.FirstOrDefaultAsync(t => t.AppointmentId == id);
            return a;
        }
        public bool CheckIfReportForAppointmentExists(Guid id)
        {
            var a = GetReportByAppointmentIdAsync(id).Result;
            return a is null ? false : true;
        }
    }
}

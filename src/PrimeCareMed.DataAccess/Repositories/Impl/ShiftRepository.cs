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
    public class ShiftRepository : IShiftRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShiftRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
        {
            return await _context.Shift.OrderBy(r => r.Id).Include(r=>r.Nurse).Include(r => r.Doctor).Include(r => r.Office).ToListAsync();
        }
        public async Task<Shift> AddAsync(Shift shiftSession)
        {
            await _context.Shift.AddAsync(shiftSession);
            await _context.SaveChangesAsync();
            return shiftSession;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Shift.FirstOrDefault(r => r.Id == id);
            _context.Shift.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Shift> GetShiftByIdAsync(string id)
        {
            return await _context.Shift.Include(r => r.Nurse).Include(r => r.Doctor).Include(r => r.Office).FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
        public async Task<IEnumerable<Shift>> GetAllShiftsForOffice(Guid id)
        {
            return GetAllShiftsAsync().Result.Where(r => r.Office.Id == id);
        }
       
        public async Task<IEnumerable<Shift>> GetAllShiftsForDoctor(string id)
        {
            return GetAllShiftsAsync().Result.Where(r => r.Doctor.Id == id);
        }
        public async Task<IEnumerable<Shift>> GetAllShiftsForNurse(string id)
        {
            return GetAllShiftsAsync().Result.Where(r => r.Nurse.Id == id);
        }
    }
}

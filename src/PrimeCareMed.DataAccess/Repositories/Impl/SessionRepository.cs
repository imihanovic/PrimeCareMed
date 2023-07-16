using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SessionRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<Session> AddAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Sessions.FirstOrDefault(r => r.Id == id);
            _context.Sessions.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Session> UpdateAsync(Session session)
        {
            var editItem = await GetSessionByIdAsync(session.Id);
            editItem.Shift = session.Shift;
            editItem.ShiftStartTime = session.ShiftStartTime;
            editItem.ShiftEndTime = session.ShiftEndTime;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Session> GetSessionByIdAsync(Guid id)
        {
            return await _context.Sessions.Include(r => r.Shift).ThenInclude(r=>r.Doctor).FirstOrDefaultAsync(t => t.Id == id);
        }
        public Session CheckIfOpenSessionExistsForDoctor(string Id)
        {
            return _context.Sessions.Include(r => r.Shift).ThenInclude(r => r.Doctor).Include(r => r.Shift).ThenInclude(r => r.Nurse).Include(r => r.Shift).ThenInclude(r => r.Office).Where(r => r.Shift.Doctor.Id == Id && r.ShiftEndTime == null).FirstOrDefault();
        }
        public Session CheckIfOpenSessionExistsForNurse(string Id)
        {
            return _context.Sessions.Include(r => r.Shift).ThenInclude(r => r.Nurse).Include(r => r.Shift).ThenInclude(r => r.Office).Include(r => r.Shift).ThenInclude(r => r.Doctor).Where(r => r.Shift.Nurse.Id == Id && r.ShiftEndTime == null).FirstOrDefault();
            
        }
        public async Task<IEnumerable<Session>> GetAllCurrentSessions()
        {
            return await _context.Sessions.Include(r => r.Shift).ThenInclude(r => r.Doctor).Include(r => r.Shift).ThenInclude(r => r.Nurse).Include(r => r.Shift).ThenInclude(r => r.Office).Where(r => r.ShiftEndTime == null).ToListAsync();
        }

    }
}

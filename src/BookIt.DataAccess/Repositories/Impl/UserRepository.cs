using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(user => user.Id).Include(user => user.Restaurant).ToListAsync();
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return  await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            var editItem = await GetUserByIdAsync(user.Id);
            editItem.UserName = user.Email;
            editItem.FirstName = user.FirstName;
            editItem.LastName = user.LastName;
            editItem.Email = user.Email;
            editItem.PhoneNumber = user.PhoneNumber;
            editItem.EmailConfirmed = true;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public  void DeleteAsync(string id)
        {
            var deleteItem = _context.Users.FirstOrDefault(user => user.Id == id);
            _context.Users.Remove(deleteItem);
            _context.SaveChanges();
        }
    }
}

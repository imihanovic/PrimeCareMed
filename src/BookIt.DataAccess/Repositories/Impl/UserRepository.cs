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
            return await _context.Users.OrderBy(user => user.Id).ToListAsync();
        }
        public ApplicationUser GetUserById(string id)
        {
            return  _context.Users.FirstOrDefault(user => user.Id == id);
        }
        public ApplicationUser Update(ApplicationUser user)
        {
            var editItem = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            editItem.FirstName = user.FirstName;
            editItem.LastName = user.LastName;
            editItem.Email = user.Email;
            editItem.PhoneNumber = user.PhoneNumber;
            _context.SaveChanges();
            return editItem;
        }
        public void Delete(string id)
        {
            var deleteItem = _context.Users.FirstOrDefault(x => x.Id == id);
            _context.Users.Remove(deleteItem);
            _context.SaveChanges();
        }
    }
}

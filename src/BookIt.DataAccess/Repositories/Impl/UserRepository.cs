using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(user => user.Id).ToListAsync();
        }
        public ApplicationUser GetUserById(string id)
        {
            return  _context.Users.FirstOrDefault(user => user.Id == id);
        }
        public void Update(ApplicationUser user)
        {
            var editItem = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            editItem.FirstName = user.FirstName;
            _context.SaveChanges();            
        }
        public void Delete(string id)
        {
            var deleteItem = _context.Users.FirstOrDefault(x => x.Id == id);
            _context.Users.Remove(deleteItem);
            _context.SaveChanges();
        }
    }
}

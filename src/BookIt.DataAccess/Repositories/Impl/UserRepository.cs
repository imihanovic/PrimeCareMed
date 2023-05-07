using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Persistence;
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
    }
}

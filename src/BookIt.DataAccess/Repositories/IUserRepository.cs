using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Enums;

namespace BookIt.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        ApplicationUser GetUserById(string id);

        void Update(ApplicationUser user);

        void Delete(string id);
    }
}

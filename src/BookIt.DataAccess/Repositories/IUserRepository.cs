using BookIt.Core.Entities.Identity;

namespace BookIt.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        ApplicationUser GetUserById(string id);

        ApplicationUser Update(ApplicationUser user);

        void Delete(string id);
    }
}

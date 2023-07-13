using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user);

        void DeleteAsync(string id);
    }
}

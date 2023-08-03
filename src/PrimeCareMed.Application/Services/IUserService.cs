using PrimeCareMed.Application.Models;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Application.Services;

public interface IUserService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);

    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);

    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);

    Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel);

    List<string> GetUserModelFields();

    IEnumerable<ListUsersModel> GetAllUsers();
    IEnumerable<ListUsersModel> UserSorting(IEnumerable<ListUsersModel> Users, string sortOrder);

    IEnumerable<ListUsersModel> UserSearch(IEnumerable<ListUsersModel> users, string searchString);

    IEnumerable<ListUsersModel> UserFilter(IEnumerable<ListUsersModel> users, string role);

    void DeleteUser(string id);

    ApplicationUser EditUser(UpdateUserModel userModel);
}

using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Users
{

    [Authorize(Policy = "RequireAdministratorRole")]
    public class ViewAllUsersModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

#nullable enable
        public PaginatedList<ListUsersModel> Users { get; set; }
#nullable disable

        public List<string> UserModelProperties;

        public int TotalPages { get; set; }
        public ViewAllUsersModel(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public void OnGet(string sort, string currentFilter, string keyword, string roleFilter, int? pageIndex)
        {
            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 1;

            var users = _userService.GetAllUsers();

            UserModelProperties = _userService.GetUserModelFields();


            ViewData["CurrentSort"] = sort;
            users = _userService.UserSorting(users, sort);

            ViewData["Keyword"] = keyword;
            users = _userService.UserSearch(users, keyword);

            ViewData["RoleFilter"] = roleFilter;
            users = _userService.UserFilter(users, roleFilter);

            Users = PaginatedList<ListUsersModel>.Create(users, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(users.Count(), pageSize));
        }
    }
}

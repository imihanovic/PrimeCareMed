using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Users
{

    [Authorize(Roles = ("Administrator, Manager"))]
    public class ViewAllUsersModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

#nullable enable
        public List<ListUsersModel> Users { get; set; }
#nullable disable

        public List<string> UserModelProperties;

        public ViewAllUsersModel(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public void OnGet(string sort, string keyword, string roleFilter)
        {
            Users =  _userService.GetAllUsers().ToList();

            ViewData["Sort"] = sort;
            UserModelProperties = _userService.GetUserModelFields();

            Users = _userService.UserSorting(Users, sort).ToList();

            ViewData["Keyword"] = keyword;
            Users = _userService.UserSearch(Users, keyword).ToList();

            ViewData["RoleFilter"] = roleFilter;
            Users = _userService.UserFilter(Users, roleFilter).ToList();
        }
    }
}

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

        public ViewAllUsersModel(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public void OnGet()
        {
            Users =  _userService.GetAllUsersAsync().ToList();
        }
    }
}

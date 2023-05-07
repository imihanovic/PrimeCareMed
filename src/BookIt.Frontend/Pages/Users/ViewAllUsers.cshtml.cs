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
        public Dictionary<ApplicationUser, string> Users { get; set; } = new Dictionary<ApplicationUser, string>();
#nullable disable

        public ViewAllUsersModel(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async void OnGet()
        {
            var users = _userService.GetAllUsersAsync().Result.ToList();
            foreach (var user in users)
            {
                try
                {
                    var role = _userManager.GetRolesAsync(user).Result.ToList().First();
                    Users.Add(user, role);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }             
            }
        }
    }
}

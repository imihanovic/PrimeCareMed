using AutoMapper;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IUserRepository _userRepository;
        private IWebHostEnvironment _webEnv;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public EditUserModel(IUserService userService,
            IUserRepository userRepository,
            IWebHostEnvironment environment,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userRepository = userRepository;
            _webEnv = environment;
            _mapper = mapper;
            _userManager = userManager;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public UpdateUserModel EditUser { get; set; }

        public void OnGet()
        {
            var user = _userRepository.GetUserById(Id);
            var role = _userManager.GetRolesAsync(user).Result.ToList().First();
            EditUser = _mapper.Map<UpdateUserModel>(user);
            EditUser.Role = role;
        }

        public async Task<IActionResult> OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditUser.Id = Id;
            var user = _userService.EditUser(EditUser);
            var role = _userManager.GetRolesAsync(user).Result.First().ToString();
            await _userManager.RemoveFromRoleAsync(user, role);
            await _userManager.AddToRoleAsync(user, EditUser.Role);

            return RedirectToPage("ViewAllUsers");
        }

        public IActionResult OnPostDelete()
        {
            _userService.DeleteUser(Id);

            return RedirectToPage("ViewAllUsers");

        }
    }
}

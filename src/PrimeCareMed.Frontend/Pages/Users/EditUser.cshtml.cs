using AutoMapper;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrimeCareMed.Frontend.Pages.Users
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        public EditUserModel(IUserService userService,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore
            )
        {
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _userStore = userStore;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public UpdateUserModel EditUser { get; set; }

        public void OnGet()
        {
            var user = _userRepository.GetUserByIdAsync(Id).Result;
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
            var user = _userRepository.GetUserByIdAsync(Id).Result;
            var role = _userManager.GetRolesAsync(user).Result.ToList().First();
            
            _userService.DeleteUser(Id);
            
            return RedirectToPage("ViewAllUsers");
        }
    }
}

using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IUserRepository _userRepository;
        private IWebHostEnvironment _webEnv;

        public EditUserModel(IUserService userService,
            IUserRepository userRepository,
            IWebHostEnvironment environment)
        {
            _userService = userService;
            _userRepository = userRepository;
            _webEnv = environment;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public ApplicationUser EditUser { get; set; }

        public void OnGet()
        {
            EditUser = _userRepository.GetUserById(Id);
        }

        public async Task<IActionResult> OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditUser.Id = Id;
            _userRepository.Update(EditUser);

            return RedirectToPage("ViewAllUsers");
        }

        public IActionResult OnPostDelete()
        {
            _userRepository.Delete(Id);

            return RedirectToPage("ViewAllUsers");

        }
    }
}

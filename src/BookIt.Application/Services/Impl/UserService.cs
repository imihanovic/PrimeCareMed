using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BookIt.Application.Common.Email;
using BookIt.Application.Exceptions;
using BookIt.Application.Helpers;
using BookIt.Application.Models;
using BookIt.Application.Models.User;
using BookIt.Application.Templates;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using System.Data;
using BookIt.Application.Models.Dish;

namespace BookIt.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ITemplateService templateService,
        IEmailService emailService,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _templateService = templateService;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel)
    {
        var user = _mapper.Map<ApplicationUser>(createUserModel);

        var result = await _userManager.CreateAsync(user, createUserModel.Password);

        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
            new Dictionary<string, string> { { "{UserId}", user.Id }, { "{Token}", token } });

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "[BookIt]Confirm your email"));

        return new CreateUserResponseModel
        {
            Id = Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id)
        };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.Username);

        if (user == null)
            throw new NotFoundException("Username or password is incorrect");

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);

        if (!signInResult.Succeeded)
            throw new BadRequestException("Username or password is incorrect");

        var token = JwtHelper.GenerateToken(user, _configuration);

        return new LoginResponseModel
        {
            Username = user.UserName,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);

        if (user == null)
            throw new UnprocessableRequestException("Your verification link is incorrect");

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);

        if (!result.Succeeded)
            throw new UnprocessableRequestException("Your verification link has expired");

        return new ConfirmEmailResponseModel
        {
            Confirmed = true
        };
    }

    public async Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException("User does not exist anymore");

        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);

        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

    public IEnumerable<ListUsersModel> GetAllUsers()
    {
        var usersFromDatabase =  _userRepository.GetAllUsersAsync().Result;
        List<ListUsersModel> users = new List<ListUsersModel>();
        foreach(var user in usersFromDatabase)
        {
            var userDto = _mapper.Map<ListUsersModel>(user);
            try
            {
                var role = _userManager.GetRolesAsync(user).Result.ToList().First();
                userDto.UserRole = role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            users.Add(userDto);
        }
        return users.AsEnumerable();
    }

    public IEnumerable<ListUsersModel> GetAllManagers()
    {
        var usersFromDatabase = _userRepository.GetAllUsersAsync().Result;
        List<ListUsersModel> managers = new List<ListUsersModel>();
        foreach (var user in usersFromDatabase)
        {
            var userDto = _mapper.Map<ListUsersModel>(user);
            try
            {
                var role = _userManager.GetRolesAsync(user).Result.ToList().First();
                userDto.UserRole = role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if(userDto.UserRole == "Manager")
            {
                managers.Add(userDto);
            }          
        }
        return managers.AsEnumerable();
    }

    public List<string> GetUserModelFields()
    {
        var userDto = new ListUsersModel();
        return userDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
    }


    public IEnumerable<ListUsersModel> UserSorting(IEnumerable<ListUsersModel> users, string sortOrder)
    {
        IEnumerable<ListUsersModel> sortedUsers = users;
        switch (sortOrder)
        {
            case "FirstName":
                return users.OrderBy(s => s.FirstName);
            case "FirstNameDesc":
                return users = users.OrderByDescending(s => s.FirstName);
            case "LastName":
                return users.OrderBy(s => s.LastName);
            case "LastNameDesc":
                return users.OrderByDescending(s => s.LastName);
            case "UserName":
                return users.OrderBy(s => s.UserName);
            case "UserNameDesc":
                return users.OrderByDescending(s => s.UserName);
            case "Email":
                return users.OrderBy(s => s.Email);
            case "EmailDesc":
                return users.OrderByDescending(s => s.Email);
            default:
                return users.OrderBy(s => s.LastName);
        }
    }

    public IEnumerable<ListUsersModel> UserSearch(IEnumerable<ListUsersModel> users, string searchString)
    {
        IEnumerable<ListUsersModel> searchedUsers = users;
        if (!String.IsNullOrEmpty(searchString))
        {
            var searchStrTrim = searchString.ToLower().Trim();
            searchedUsers = users.Where(s => s.LastName.ToLower().Contains(searchStrTrim)
                                        || s.FirstName.ToLower().Contains(searchStrTrim)
                                        || s.UserName.ToLower().Contains(searchStrTrim)
                                        || s.Email.ToLower().Contains(searchStrTrim)
                                        || s.PhoneNumber.ToLower().Contains(searchStrTrim)
                                        );
        }
        return searchedUsers;
    }

    public IEnumerable<ListUsersModel> UserFilter(IEnumerable<ListUsersModel> users, string role)
    {
        IEnumerable<ListUsersModel> filteredUsers = users;
        if (!String.IsNullOrEmpty(role))
        {
            var roleTrim = role.ToLower().Trim();
            filteredUsers = users.Where(s => s.UserRole.ToLower() == roleTrim);
        }
        return filteredUsers;
    }

    public void DeleteUser(string id)
    {
        _userRepository.DeleteAsync(id);
    }

    public ApplicationUser EditUser(UpdateUserModel userModel)
    {
        var user = _mapper.Map<ApplicationUser>(userModel);
        return  _userRepository.UpdateAsync(user).Result;
    }

}

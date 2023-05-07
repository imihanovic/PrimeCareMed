﻿using BookIt.Application.Models;
using BookIt.Application.Models.User;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Enums;

namespace BookIt.Application.Services;

public interface IUserService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);

    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);

    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);

    Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel);

    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

}
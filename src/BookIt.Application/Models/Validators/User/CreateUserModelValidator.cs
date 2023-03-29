﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using BookIt.Application.Models.User;
using BookIt.DataAccess.Identity;

namespace BookIt.Application.Models.Validators.User;

public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    private readonly UserManager<DataAccess.Identity.User> _userManager;

    public CreateUserModelValidator(UserManager<DataAccess.Identity.User> userManager)
    {
        _userManager = userManager;

        RuleFor(u => u.Username)
            .MinimumLength(UserValidatorConfiguration.MinimumUsernameLength)
            .WithMessage($"Username should have minimum {UserValidatorConfiguration.MinimumUsernameLength} characters")
            .MaximumLength(UserValidatorConfiguration.MaximumUsernameLength)
            .WithMessage($"Username should have maximum {UserValidatorConfiguration.MaximumUsernameLength} characters")
            .MustAsync(UsernameIsUniqueAsync)
            .WithMessage("Username is not available");

        RuleFor(u => u.Password)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters");

        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("Email address is not valid")
            .MustAsync(EmailAddressIsUniqueAsync)
            .WithMessage("Email address is already in use");
    }

    private async Task<bool> EmailAddressIsUniqueAsync(string email, CancellationToken cancellationToken = new())
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user == null;
    }

    private async Task<bool> UsernameIsUniqueAsync(string username, CancellationToken cancellationToken = new())
    {
        var user = await _userManager.FindByNameAsync(username);

        return user == null;
    }
}

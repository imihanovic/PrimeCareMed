using AutoMapper;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
        CreateMap<ApplicationUser, ListUsersModel>();
        CreateMap<ApplicationUser, UpdateUserModel>();
        CreateMap<UpdateUserModel, ApplicationUser>();
    }
}

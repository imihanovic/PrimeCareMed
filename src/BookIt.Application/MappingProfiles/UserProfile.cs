using AutoMapper;
using BookIt.Application.Models.User;
using BookIt.Core.Entities.Identity;

namespace BookIt.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
        CreateMap<IEnumerable<ApplicationUser>, IEnumerable<UserResponseModel>>();
    }
}

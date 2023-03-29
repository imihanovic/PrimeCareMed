using AutoMapper;
using BookIt.Application.Models.User;
using BookIt.DataAccess.Identity;

namespace BookIt.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, User>();
    }
}

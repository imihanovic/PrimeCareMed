using AutoMapper;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class CheckupProfile : Profile
    {
        public CheckupProfile() 
        {
            CreateMap<CheckupModelForCreate, Checkup>();
            CreateMap<Checkup, CheckupModelForCreate>();
            CreateMap<Checkup, CheckupModel>();
            CreateMap<CheckupModel, Checkup>();
            CreateMap<CheckupModelForCreate, CheckupModel>();
            CreateMap<CheckupModel, CheckupModelForCreate>();
        }
    }
}

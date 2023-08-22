using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Core.Entities;
using AutoMapper;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class HospitalProfile : Profile
    {
        public HospitalProfile() 
        {
            CreateMap<HospitalModelForCreate, Hospital>();
            CreateMap<Hospital, HospitalModelForCreate>();
            CreateMap<Hospital, HospitalModel>();
            CreateMap<HospitalModel, Hospital>();
            CreateMap<HospitalModelForCreate, HospitalModel>();
            CreateMap<HospitalModel, HospitalModelForCreate>();
        }
    }
}

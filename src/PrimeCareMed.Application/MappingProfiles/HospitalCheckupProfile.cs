using AutoMapper;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class HospitalCheckupProfile : Profile
    {
        public HospitalCheckupProfile()
        {
            CreateMap<HospitalCheckupModelForCreate, HospitalCheckup>();
            CreateMap<Hospital, HospitalCheckupModelForCreate>();
        }
    }
}

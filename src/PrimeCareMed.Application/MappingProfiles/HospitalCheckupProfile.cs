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
            CreateMap<HospitalCheckup, HospitalCheckupModelForCreate>();
            CreateMap<HospitalCheckupModelForCreate, HospitalCheckupModel>();
            CreateMap<HospitalCheckupModel, HospitalCheckupModelForCreate>();
            CreateMap<HospitalCheckupModel, HospitalCheckup>();
            CreateMap<Hospital, HospitalCheckupModel>();
        }
    }
}

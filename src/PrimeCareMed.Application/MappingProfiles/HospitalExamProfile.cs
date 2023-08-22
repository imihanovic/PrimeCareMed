using AutoMapper;
using PrimeCareMed.Application.Models.HospitalExam;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class HospitalExamProfile : Profile
    {
        public HospitalExamProfile()
        {
            CreateMap<HospitalExamModelForCreate, HospitalExam>();
            CreateMap<Hospital, HospitalExamModelForCreate>();
        }
    }
}

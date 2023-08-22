using AutoMapper;
using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile() 
        {
            CreateMap<ExamModelForCreate, Exam>();
            CreateMap<Exam, ExamModelForCreate>();
            CreateMap<Exam, ExamModel>();
            CreateMap<ExamModel, Exam>();
            CreateMap<ExamModelForCreate, ExamModel>();
            CreateMap<ExamModel, ExamModelForCreate>();
        }
    }
}

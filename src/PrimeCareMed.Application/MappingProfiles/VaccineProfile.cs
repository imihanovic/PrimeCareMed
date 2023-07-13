using AutoMapper;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class VaccineProfile : Profile
    {
        public VaccineProfile()
        {
            CreateMap<VaccineModel, Vaccine>();
            CreateMap<Vaccine, VaccineModel>();
            CreateMap<VaccineModelForCreate, Vaccine>();
            CreateMap<Vaccine, VaccineModelForCreate>();
            CreateMap<VaccineModel, VaccineModelForCreate>();
            CreateMap<VaccineModelForCreate, VaccineModel>();
        }
    }
}

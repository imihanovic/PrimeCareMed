using AutoMapper;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class ShiftProfile : Profile
    {
        public ShiftProfile()
        {
            CreateMap<ShiftModelForCreate, Shift>();
            CreateMap<Shift, ShiftModelForCreate>();
            CreateMap<Shift, ShiftModel>();
            CreateMap<ShiftModel, Shift>();
            CreateMap<ShiftModelForCreate, Shift>();
            CreateMap<ShiftModel, ShiftModelForCreate>();
        }
    }
}

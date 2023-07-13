using AutoMapper;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class MedicinePrescriptionProfile : Profile
    {
        public MedicinePrescriptionProfile()
        {
            CreateMap<MedicinePrescriptionModelForCreate, MedicinePrescription>();
            CreateMap<MedicinePrescription, MedicinePrescriptionModelForCreate>();
            CreateMap<MedicinePrescription, MedicinePrescriptionModel>();
            CreateMap<MedicinePrescriptionModel, MedicinePrescription>();
            CreateMap<MedicinePrescriptionModel, MedicinePrescriptionModelForCreate>();
            CreateMap<MedicinePrescriptionModelForCreate, MedicinePrescriptionModel>();
        }
    }
}

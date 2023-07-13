using AutoMapper;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<OfficeModelForCreate, GeneralMedicineOffice>();
            CreateMap<GeneralMedicineOffice, OfficeModelForCreate>();
            CreateMap<GeneralMedicineOffice, OfficeModel>();
            CreateMap<MedicineModel, GeneralMedicineOffice>();
            CreateMap<OfficeModelForCreate, OfficeModel>();
            CreateMap<OfficeModel, OfficeModelForCreate>();
        }
    }
}

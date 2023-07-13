using AutoMapper;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class MedicineProfile : Profile
    {
        public MedicineProfile()
        {
            CreateMap<MedicineModelForCreate, Medicine>();
            CreateMap<Medicine, MedicineModelForCreate>();
            CreateMap<Medicine, MedicineModel>();
            CreateMap<MedicineModel, Medicine>();
            CreateMap<MedicineModel, MedicineModelForCreate>();
            CreateMap<MedicineModelForCreate, MedicineModel>();
        }
    }
}

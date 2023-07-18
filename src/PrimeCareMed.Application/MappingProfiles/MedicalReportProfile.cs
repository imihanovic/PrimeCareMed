using AutoMapper;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.MedicalReport;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class MedicalReportProfile : Profile
    {
        public MedicalReportProfile()
        {
            CreateMap<MedicalReportModelForCreate, MedicalReportModel>();
            CreateMap<MedicalReportModel, MedicalReportModelForCreate>();
            CreateMap<MedicalReportModel, MedicalReport>();
            CreateMap<MedicalReport, MedicalReportModel>();
            CreateMap<MedicalReportModelForCreate, MedicalReport>();
            CreateMap<MedicalReport, MedicalReportModelForCreate>();
        }
    }
}

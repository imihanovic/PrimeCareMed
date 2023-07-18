using AutoMapper;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class PatientVaccineProfile : Profile
    {
        public PatientVaccineProfile()
        {
            CreateMap<PatientVaccineModelForCreate, PatientsVaccine>();
            CreateMap<PatientsVaccine, PatientVaccineModelForCreate>();
            CreateMap<PatientsVaccine, PatientVaccineModel>();
            CreateMap<PatientVaccineModel, PatientsVaccine>();
            CreateMap<PatientVaccineModel, PatientVaccineModelForCreate>();
            CreateMap<PatientVaccineModelForCreate, PatientVaccineModel>();
        }
    }
}

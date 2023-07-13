using AutoMapper;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<PatientModel, Patient>();
            CreateMap<Patient, PatientModel>();
            CreateMap<PatientModelForCreate, Patient>();
            CreateMap<Patient, PatientModelForCreate>();
            CreateMap<PatientModel, PatientModelForCreate>();
            CreateMap<PatientModelForCreate, PatientModel>();
        }
    }
}

using AutoMapper;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class CheckupAppointmentProfile : Profile
    {
        public CheckupAppointmentProfile() 
        {
            CreateMap<CheckupAppointmentModelForCreate, CheckupAppointment>();
            CreateMap<CheckupAppointment, CheckupAppointmentModelForCreate>();
            CreateMap<CheckupAppointment, CheckupAppointmentModel>();
            CreateMap<CheckupAppointmentModel, CheckupAppointment>();
            CreateMap<CheckupAppointmentModelForCreate, CheckupAppointmentModel>();
            CreateMap<CheckupAppointmentModel, CheckupAppointmentModelForCreate>();
        }
    }
}

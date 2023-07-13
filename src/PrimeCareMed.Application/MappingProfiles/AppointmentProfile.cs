using AutoMapper;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.MappingProfiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentModelForCreate, Appointment>();
            CreateMap<Appointment, AppointmentModelForCreate>();
            CreateMap<Appointment, AppointmentModel>();
            CreateMap<AppointmentModel, Appointment>();
            CreateMap<AppointmentModel, AppointmentModelForCreate>();
            CreateMap<AppointmentModelForCreate, AppointmentModel>();
            CreateMap<AppointmentDetailsModel, Appointment>();
            CreateMap<Appointment, AppointmentDetailsModel>();
        }
    }
}

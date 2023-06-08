using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationModelForCreate, Reservation>();
            CreateMap<Reservation, ReservationModelForCreate>();
            CreateMap<Reservation, ReservationModelForUpdate>();
            CreateMap<ReservationModelForUpdate, Reservation>();
        }
    }
}

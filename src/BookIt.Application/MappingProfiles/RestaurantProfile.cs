using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile() 
        {
            CreateMap<RestaurantModelForCreate, Restaurant>();
            CreateMap<Restaurant, RestaurantModelForCreate>();
            CreateMap<RestaurantModel, Restaurant>();
            CreateMap<Restaurant, RestaurantModel>();
        }
    }
}

using AutoMapper;
using BookIt.Application.Models.RestaurantDish;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles
{
    public class RestaurantDishProfile : Profile
    {
        public RestaurantDishProfile()
        {
            CreateMap<RestaurantDishModel, RestaurantDish>();
            CreateMap<RestaurantDish, RestaurantDishModel>();
            CreateMap<RestaurantDish, RestaurantDishModelForCreate>();
            CreateMap<RestaurantDishModelForCreate, RestaurantDish>();
            CreateMap<RestaurantDish, RestaurantDishModelForUpdate>();
            CreateMap<RestaurantDishModelForUpdate, RestaurantDish>();
        }
    }
}

using AutoMapper;
using BookIt.Application.Models.Dish;
using BookIt.Application.Models.Table;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<DishModelForCreate, Dish>();
            CreateMap<Dish, DishModelForCreate>();
            CreateMap<DishModel, Dish>();
            CreateMap<Dish, DishModel>();
        }
    }
}

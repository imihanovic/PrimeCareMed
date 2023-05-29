using AutoMapper;
using BookIt.Application.Models.Dish;
using BookIt.Application.Models.Table;
using BookIt.DataAccess.Repositories;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories.Impl;
using BookIt.Application.Models.Restaurant;

namespace BookIt.Application.Services.Impl
{
    public class DishService : IDishService
    {
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public DishService(IMapper mapper,
            IDishRepository dishRepository)
        {
            _mapper = mapper;
            _dishRepository = dishRepository;
            
        }
        public async Task<DishModel> AddAsync(DishModelForCreate createDishModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<DishModelForCreate, Dish>();

            });
            var dish = config.CreateMapper().Map<Dish>(createDishModel);

            await _dishRepository.AddAsync(dish);
            return _mapper.Map<DishModel>(dish);
        }
        public IEnumerable<DishModel> GetAllDishes()
        {
            var dishesFromDB = _dishRepository.GetAllDishesAsync().Result;

            List<DishModel> dishes = new List<DishModel>();
            foreach (var dish in dishesFromDB)
            {
                var dishDto = _mapper.Map<DishModel>(dish);
                dishDto.DishName = dish.DishName;
                dishDto.DishDescription = dish.DishDescription;
                dishDto.Category = dish.Category.ToString();
                dishes.Add(dishDto);
            }
            return dishes.AsEnumerable();
        }

        public List<string> GetDishModelFields()
        {
            var dishDto = new DishModel();
            return dishDto.GetType().GetProperties().Where(x => x.Name != "DishDescription" && x.Name != "Category").Select(x => x.Name).ToList();
        }
        public IEnumerable<DishModel> DishSearch(IEnumerable<DishModel> dishes, string searchString)
        {
            IEnumerable<DishModel> searchedDishes = dishes;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedDishes = dishes.Where(s => s.DishName.ToLower().Contains(searchStrTrim)
                                            || s.DishDescription.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedDishes;
        }
        public async Task DeleteDishAsync(Guid Id)
        {
            await _dishRepository.DeleteAsync(Id);

        }
    }
}

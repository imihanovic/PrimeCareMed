using AutoMapper;
using BookIt.Application.Models.RestaurantDish;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using BookIt.Application.Models.Dish;

namespace BookIt.Application.Services.Impl
{
    public class RestaurantDishService : IRestaurantDishService
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantDishRepository _restaurantDishRepository;

        public RestaurantDishService(IMapper mapper,
            IRestaurantRepository restaurantRepository,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IDishRepository dishRepository,
            IRestaurantDishRepository restaurantDishRepository)
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _dishRepository = dishRepository;
            _restaurantDishRepository = restaurantDishRepository;
        }
        public async Task<RestaurantDishModel> AddAsync(RestaurantDishModelForCreate createRestaurantDishModel)
        {
            var restaurantDish = _mapper.Map<RestaurantDish>(createRestaurantDishModel);
            restaurantDish.Restaurant = await _restaurantRepository.GetRestaurantByIdAsync(Guid.Parse(createRestaurantDishModel.RestaurantId));
            restaurantDish.Dish = _dishRepository.GetAllDishesAsync().Result.FirstOrDefault(d => d.Id == Guid.Parse(createRestaurantDishModel.DishId));
            restaurantDish.IsAvailable = true;

            await _restaurantDishRepository.AddAsync(restaurantDish);
            return _mapper.Map<RestaurantDishModel>(restaurantDish);
        }

        public IEnumerable<RestaurantDishModel> GetAllRestaurantDishesByRestaurantId(string restaurantId)
        {
            var restaurantDishesFromDatabase = _restaurantDishRepository.GetAllRestaurantDishesAsync(restaurantId).Result;

            List<RestaurantDishModel> restaurantDishes = new List<RestaurantDishModel>();
            foreach (var restaurantDish in restaurantDishesFromDatabase)
            {
                var restaurantDishDto = _mapper.Map<RestaurantDishModel>(restaurantDish);
                restaurantDishDto.RestaurantId = restaurantDish.Restaurant.Id.ToString();
                restaurantDishDto.RestaurantOwner = restaurantDish.Restaurant.RestaurantOwner;
                restaurantDishDto.RestaurantName = restaurantDish.Restaurant.RestaurantName;
                restaurantDishDto.Address = restaurantDish.Restaurant.Address;
                restaurantDishDto.DishId = restaurantDish.Dish.Id.ToString();
                restaurantDishDto.DishName = restaurantDish.Dish.DishName;
                restaurantDishDto.DishDescription = restaurantDish.Dish.DishDescription;
                restaurantDishDto.Category = restaurantDish.Dish.Category.ToString();
                restaurantDishDto.Price = restaurantDish.Price;
                restaurantDishDto.IsAvailable = restaurantDish.IsAvailable;
                restaurantDishes.Add(restaurantDishDto);
            }
            return restaurantDishes.AsEnumerable();
        }

        public RestaurantDish EditRestaurantDish(RestaurantDishModelForUpdate restaurantDishModel)
        {
            var restaurantDish = _mapper.Map<RestaurantDish>(restaurantDishModel);
            return _restaurantDishRepository.UpdateAsync(restaurantDish).Result;
        }

        public List<string> GetRestaurantModelFields(string userRole)
        {
            var restaurantDishDto = new RestaurantDishModel();
            if(userRole == "Administrator" || userRole == "Manager")
            {
                return restaurantDishDto.GetType().GetProperties().Where(x => x.Name != "RestaurantId" && x.Name != "Id" && x.Name != "DishId" && x.Name != "RestaurantOwner" && x.Name != "Address" && x.Name != "RestaurantName").Select(x => x.Name).ToList();
            }
            else
            {
                return restaurantDishDto.GetType().GetProperties().Where(x => x.Name != "RestaurantId" && x.Name != "Id" && x.Name != "DishId" && x.Name != "RestaurantOwner" && x.Name != "Address" && x.Name != "RestaurantName" && x.Name != "IsAvailable").Select(x => x.Name).ToList();
            }
            
        }

        public IEnumerable<RestaurantDishModel> RestaurantSorting(IEnumerable<RestaurantDishModel> restaurantDishes, string sortOrder)
        {
            IEnumerable<RestaurantDishModel> sortedRestaurantDishes = restaurantDishes;
            switch (sortOrder)
            {
                case "DishName":
                    return restaurantDishes.OrderBy(r => r.DishName);
                case "DishNameDesc":
                    return restaurantDishes.OrderByDescending(r => r.DishName);
                case "Price":
                    return restaurantDishes.OrderBy(r => r.Price);
                case "PriceDesc":
                    return restaurantDishes.OrderByDescending(r => r.Price);
                default:
                    return restaurantDishes.OrderBy(r => r.DishName);
            }
        }

        public IEnumerable<RestaurantDishModel> RestaurantDishSearch(IEnumerable<RestaurantDishModel> restaurantDishes, string searchString)
        {
            IEnumerable<RestaurantDishModel> searchedRestaurantsDishes = restaurantDishes;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedRestaurantsDishes = restaurantDishes.Where(s => s.DishName.ToLower().Contains(searchStrTrim)
                                            || s.DishDescription.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedRestaurantsDishes;
        }

        public IEnumerable<RestaurantDishModel> RestaurantDishFilter(IEnumerable<RestaurantDishModel> restaurantDishes, string category)
        {
            IEnumerable<RestaurantDishModel> filtratedDishes = restaurantDishes;
            if (!String.IsNullOrEmpty(category))
            {
                var categoryTrim = category.ToLower().Trim();
                filtratedDishes = restaurantDishes.Where(t => t.Category.ToLower() == categoryTrim);
            }
            return filtratedDishes;
        }

        public async Task DeleteRestaurantDishAsync(Guid Id)
        {
            await _restaurantDishRepository.DeleteAsync(Id);
        }
    }
}

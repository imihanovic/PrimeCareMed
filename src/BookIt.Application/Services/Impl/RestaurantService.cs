using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookIt.Application.Services.Impl
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurantService(IMapper mapper,
            IRestaurantRepository restaurantRepository,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager) 
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<RestaurantModel> AddAsync(RestaurantModelForCreate createRestaurantModel)
        {
            var restaurant = _mapper.Map<Restaurant>(createRestaurantModel);
            await _restaurantRepository.AddAsync(restaurant);
            return _mapper.Map<RestaurantModel>(restaurant);
        }

        public IEnumerable<RestaurantModel> GetAllRestaurants()
        {
            var restaurantsFromDatabase = _restaurantRepository.GetAllRestaurantsAsync().Result;
            
            List<RestaurantModel> restaurants = new List<RestaurantModel>();
            foreach (var restaurant in restaurantsFromDatabase)
            {
                var manager = _userRepository.GetUserByIdAsync(restaurant.ManagerId).Result;
                var restaurantModel = _mapper.Map<RestaurantModel>(restaurant);
                restaurantModel.ManagerFirstName = manager.FirstName;
                restaurantModel.ManagerLastName = manager.LastName;
                restaurantModel.ManagerUserName = manager.UserName;
                restaurants.Add(restaurantModel);
            }
            return restaurants.AsEnumerable();
        }
        public Restaurant EditRestaurantAsync(RestaurantModelForCreate restaurantModel)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantModel);
            return _restaurantRepository.UpdateAsync(restaurant).Result;
        }

        public async Task DeleteRestaurantAsync(Guid Id)
        {
            await _restaurantRepository.DeleteAsync(Id);

        }

        public List<string> GetRestaurantModelFields()
        {
            var restaurantDto = new RestaurantModel();
            return restaurantDto.GetType().GetProperties().Where(x => x.Name != "ManagerId" && x.Name != "Id" && x.Name != "ManagerFirstName" && x.Name != "ManagerLastName").Select(x => x.Name).ToList();
        }

        public IEnumerable<RestaurantModel> RestaurantSorting(IEnumerable<RestaurantModel> restaurants, string sortOrder)
        {
            IEnumerable<RestaurantModel> sortedRestaurants = restaurants;
            switch (sortOrder)
            {
                case "RestaurantOwner":
                    return restaurants.OrderBy(r => r.RestaurantOwner);
                case "RestaurantOwnerDesc":
                    return restaurants = restaurants.OrderByDescending(r => r.RestaurantOwner);
                case "RestaurantName":
                    return restaurants.OrderBy(r => r.RestaurantName);
                case "RestaurantNameDesc":
                    return restaurants.OrderByDescending(r => r.RestaurantName);
                case "ManagerFirstName":
                    return restaurants.OrderBy(r => r.ManagerFirstName);
                case "ManagerFirstNameDesc":
                    return restaurants.OrderByDescending(r => r.ManagerFirstName);
                case "ManagerLastName":
                    return restaurants.OrderBy(r => r.ManagerLastName);
                case "ManagerLastNameDesc":
                    return restaurants.OrderByDescending(r => r.ManagerLastName);
                case "ManagerUserName":
                    return restaurants.OrderBy(r => r.ManagerUserName);
                case "ManagerUserNameDesc":
                    return restaurants.OrderByDescending(r => r.ManagerUserName);
                default:
                    return restaurants.OrderBy(r => r.RestaurantName);
            }
        }

        public IEnumerable<RestaurantModel> RestaurantSearch(IEnumerable<RestaurantModel> restaurants, string searchString)
        {
            IEnumerable<RestaurantModel> searchedRestaurants = restaurants;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedRestaurants = restaurants.Where(s => s.RestaurantOwner.ToLower().Contains(searchStrTrim)
                                            || s.RestaurantName.ToLower().Contains(searchStrTrim)
                                            || s.Address.ToLower().Contains(searchStrTrim)
                                            || s.ManagerFirstName.ToLower().Contains(searchStrTrim)
                                            || s.ManagerLastName.ToLower().Contains(searchStrTrim)
                                            || s.ManagerUserName.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedRestaurants;
        }
    }
}

using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.User;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using BookIt.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Identity;

namespace BookIt.Application.Services.Impl
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IMapper mapper, IRestaurantRepository restaurantRepository) 
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
        }
        public async Task<RestaurantModel> AddAsync(RestaurantModelForCreate createRestaurantModel)
        {
            var restaurant = _mapper.Map<Restaurant>(createRestaurantModel);
            await _restaurantRepository.AddAsync(restaurant);
            return _mapper.Map<RestaurantModel>(restaurant);
        }

        public IEnumerable<RestaurantModel> GetAllRestaurant()
        {
            var restaurantsFromDatabase = _restaurantRepository.GetAllRestaurantsAsync().Result;
            List<RestaurantModel> restaurants = new List<RestaurantModel>();
            foreach (var restaurant in restaurantsFromDatabase)
            {
                restaurants.Add(_mapper.Map<RestaurantModel>(restaurant));
            }
            return restaurants.AsEnumerable();
        }
    }
}

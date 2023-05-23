using BookIt.Application.Models.User;

namespace BookIt.Application.Models.Restaurant
{
    public class RestaurantModel : BaseResponseModel
    {
        public string RestaurantOwner { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string ManagerId { get; set; }
    }
}

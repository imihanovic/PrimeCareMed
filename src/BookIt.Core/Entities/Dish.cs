using BookIt.Core.Common;
using BookIt.Core.Enums;

namespace BookIt.Core.Entities
{
    public class Dish : BaseEntity
    {
        public string DishName { get; set; }
        public string DishDescription { get; set;}
        public DishCategory Category { get; set;}
        public ICollection<RestaurantDish> RestaurantDishes { get; set; } = new List<RestaurantDish>();
    }
}

using BookIt.Core.Common;

namespace BookIt.Core.Entities
{
    public class RestaurantDish : BaseEntity
    {
        public Dish Dish { get; set; }

        public Restaurant Restaurant { get; set; }

        public bool IsAvailable { get; set; }

        public float Price { get; set; }

    }
}

using BookIt.Core.Common;
using BookIt.Core.Entities.Identity;

namespace BookIt.Core.Entities
{
    public class Restaurant : BaseEntity
    {
        public string RestaurantOwner { get; set; }
        public string RestaurantName { get;set; }
        public string Address { get; set; }
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }

        public ICollection<RestaurantDish> RestaurantDishes { get; set; } = new List<RestaurantDish>();

        public ICollection<Table> Tables { get; set; } = new List<Table>();
    }
}

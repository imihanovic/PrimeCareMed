namespace BookIt.Application.Models.RestaurantDish
{
    public class RestaurantDishModel : BaseResponseModel
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantOwner { get; set; }
        public string Address { get; set; }
        public string DishId { get; set; }
        public string DishName { get; set; }
        public string DishDescription { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public float Price { get; set; }
    }
}

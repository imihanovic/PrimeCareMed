using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookIt.Application.Models.RestaurantDish
{
    public class RestaurantDishModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Is available?*")]
        public bool IsAvailable { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price*")]
        public string Price { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Restaurant*")]
        public string RestaurantId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dish*")]
        public string DishId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookIt.Application.Models.Dish
{
    public class DishModelForCreate
    {

        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dish name*")]
        public string DishName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dish description*")]
        public string DishDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dish category*")]
        public string Category { get; set; }

    }
}

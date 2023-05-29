using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt.Application.Models.Dish
{
    public class DishModel : BaseResponseModel
    {
        public string DishName { get; set; }

        public string DishDescription { get; set; }

        public string Category { get; set; }
    }
}

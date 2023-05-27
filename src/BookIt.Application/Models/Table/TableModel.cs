namespace BookIt.Application.Models.Table
{
    public class TableModel : BaseResponseModel
    {
        public string TableName { get; set; }
        public int NumberOfSeats { get; set; }

        public string Area { get; set; }

        public string Smoking { get; set; }

        public string RestaurantOwner { get; set; }

        public string RestaurantName { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string RestaurantId { get; set; }
    }
}

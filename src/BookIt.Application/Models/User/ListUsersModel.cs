namespace BookIt.Application.Models.User
{
    public class ListUsersModel : BaseResponseModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserRole { get; set; }
    }
}

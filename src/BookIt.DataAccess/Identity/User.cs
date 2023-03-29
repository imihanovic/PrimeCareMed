using Microsoft.AspNetCore.Identity;

namespace BookIt.DataAccess.Identity;

public class User : IdentityUser 
{
    public string Name { get; set; }

    public string Surname { get; set; }

}

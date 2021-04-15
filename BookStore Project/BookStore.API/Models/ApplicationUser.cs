using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Models
{
    /// <summary>
    /// This class is uses for configures our Application Users.
    /// We can add additional properties like FirstName and LastName of the user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

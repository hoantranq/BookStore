using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Helpers.Seeding
{
    public class BookStoreDatabaseInitializer
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.User.ToString()));

            // Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = Authorization.DEFAULT_USERNAME,
                Email = Authorization.DEFAULT_EMAIL,
                EmailConfirmed = true,
            };

            if (userManager.Users.All(user => user.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, Authorization.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(defaultUser, Authorization.DEFAULT_ROLE.ToString());
            }
        }
    }
}

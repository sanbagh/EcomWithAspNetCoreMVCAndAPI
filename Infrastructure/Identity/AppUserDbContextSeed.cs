using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUserDbContextSeed
    {
        public async static Task SeedUserDataAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Admin",
                    UserName = "Admin",
                    Email = "sanjeev1baghel@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        City = "Agra",
                        State = "UP",
                        Country = "India",
                        Street = "10 street",
                        ZipCode = "282002",
                    }
                };

                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public static class UserManagerExt
    {
        public static async Task<AppUser> GetUserWithAddressAsync(this UserManager<AppUser> manger, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            return await manger.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
        }
        public static async Task<IdentityResult> UpdateUserWithAddressAsync(this UserManager<AppUser> manger, ClaimsPrincipal user, Address address)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var userToUpdate = await manger.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            userToUpdate.Address = address;
            return await manger.UpdateAsync(userToUpdate);
        }
        public static async Task<AppUser> FindByEmailClaimAsync(this UserManager<AppUser> manger, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            return await manger.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
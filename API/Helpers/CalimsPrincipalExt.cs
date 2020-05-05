using System.Linq;
using System.Security.Claims;

namespace API.Helpers
{
    public static class CalimsPrincipalExt
    {
        public static string GetEmailClaimValue(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
        }
    }
}
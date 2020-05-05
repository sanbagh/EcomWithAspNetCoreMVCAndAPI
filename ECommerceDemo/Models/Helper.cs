using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceDemo.Models
{
    public class Helper
    {
        public static IHttpContextAccessor HttpContextAccessor;
        public static IEnumerable<Claim> GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            if (decodedToken != null)
            {
                return decodedToken.Claims;
            }
            return null;
        }
        public static string GetDisplayName(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            if (decodedToken != null)
            {
                return decodedToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.GivenName).Value;
            }
            return null;
        }
        public static bool IsAuthenticated()
        {
            if (HttpContextAccessor == null) return false;
            return HttpContextAccessor.HttpContext.Session.GetString("token") != null;
        }
    }
}

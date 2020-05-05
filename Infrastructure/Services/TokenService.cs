using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : IToken
    {
        private SymmetricSecurityKey _key;
        private readonly IConfiguration _config;
        private UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["token:key"]));
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName),
            };

            claims.AddRange(await GetRoleAsync(user));
            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var desc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
                Issuer = _config["token:issuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(desc);
            return tokenHandler.WriteToken(token);
        }
        private async Task<IList<Claim>> GetRoleAsync(AppUser user)
        {
            var lst = new List<Claim>();
            var result = await _userManager.GetRolesAsync(user);
            if (result != null)
            {
                foreach (var claim in result)
                {
                    lst.Add(new Claim(ClaimTypes.Role, claim));
                }
            }
            return lst;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;

namespace OurSite.Core.Utilities
{
    public static class AuthenticationHelper
    {
        public static string GenerateUserToken(User user,int Expire)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sajjadhaniehfaezeherfanmobinsinamehdi"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOption = new JwtSecurityToken(
                issuer: PathTools.Domain,
                claims: new List<Claim>()
                {
                                new Claim(ClaimTypes.Name, String.Concat(user.FirstName,user.LastName)),
                                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                                new Claim(ClaimTypes.Role,"Customer")

                },
                expires: DateTime.Now.AddDays(Expire),
                signingCredentials: signInCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
            return token;
        }

        public static string GenerateAdminToken(Admin admin,Role role, int Expire)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sajjadhaniehfaezeherfanmobinsinamehdi"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOption = new JwtSecurityToken(
                issuer: PathTools.Domain,
                claims: new List<Claim>()
                {
                                new Claim(ClaimTypes.Name, String.Concat(admin.FirstName,admin.LastName)),
                                new Claim(ClaimTypes.NameIdentifier,admin.Id.ToString()),
                                new Claim(ClaimTypes.Role,role.Name)

                },
                expires: DateTime.Now.AddDays(Expire),
                signingCredentials: signInCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
            return token;
        }
    }
}

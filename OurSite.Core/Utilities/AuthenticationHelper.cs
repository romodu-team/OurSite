using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;

namespace OurSite.Core.Utilities
{
    public class AuthenticationHelper
    {
       private readonly IRoleService _roleService;
        public AuthenticationHelper(IRoleService roleService)
        {
            _roleService=roleService;
        }
        public  string GenerateUserToken(User user,int Expire)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Wx7Xl6rPABrWvLbLaXoBcaLQ8nQJg7L1Dce3zfE0"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOption = new JwtSecurityToken(
                issuer: PathTools.Domain,
                claims: new List<Claim>()
                {
                                new Claim(ClaimTypes.Name, String.Concat(user.FirstName,user.LastName)),
                                new Claim(ClaimTypes.NameIdentifier,user.UUID.ToString()),
                                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                                new Claim(ClaimTypes.Role,"Customer")

                },
                expires: DateTime.Now.AddDays(Expire),
                signingCredentials: signInCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
            return $"Bearer {token}";
        }

        public  async Task<string> GenerateAdminToken(Admin admin,Role role, int Expire)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Wx7Xl6rPABrWvLbLaXoBcaLQ8nQJg7L1Dce3zfE0"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
              var JwtTokenHandler = new JwtSecurityTokenHandler();
            //get all user claims
            var claims =await AllValidClaims(admin);
            var tokenDescriptor  = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                Issuer=PathTools.Domain
                ,
                Expires=DateTime.Now.AddHours(3),
                SigningCredentials=new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256)
              
        };

            var Token = JwtTokenHandler.CreateToken(tokenDescriptor);
            var JwtToken = JwtTokenHandler.WriteToken(Token);

            return $"Bearer {JwtToken}" ;
        }

        private async Task<List<Claim>> AllValidClaims(Admin admin)
        {
            var _options = new IdentityOptions();

            var claims= new List<Claim>()
            {
                    new Claim(ClaimTypes.NameIdentifier,admin.UUID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub,admin.Email),
                    new Claim(ClaimTypes.Sid,admin.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,admin.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
            };
            //getting the claims that we have assigned to the user
            //var userClaims = await _userManager.GetClaimsAsync(user);
           // claims.AddRange(userClaims);
            //get the user roles and add it to claims
               var userRoles = await _roleService.GetAdminRole(admin.Id);

            
              
                
                if (userRoles != null)
                {
                      claims.Add(new Claim(ClaimTypes.Role, userRoles.Name));
                    var roleClaim = await _roleService.GetSelectedPermissionOfRole(userRoles.Id);
                    foreach (var RoleClaim in roleClaim)
                    {
                        claims.Add(new Claim(RoleClaim.PermissionName,RoleClaim.PermissionName));
                    }
                }
           

            return claims;
        }
    }
}

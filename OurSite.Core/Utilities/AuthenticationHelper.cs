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
using OurSite.DataLayer.Interfaces;
using OurSite.Core.DTOs;
using OurSite.DataLayer.Repositories;

namespace OurSite.Core.Utilities
{
    public class AuthenticationHelper
    {
        private readonly IRoleService _roleService;
        private IGenericRepository<RefreshToken> _RefreshTokenRepository;
        public AuthenticationHelper(IRoleService roleService, IGenericRepository<RefreshToken> RefreshTokenRepository)
        {
            _RefreshTokenRepository = RefreshTokenRepository;
            _roleService = roleService;
        }
        public async Task<AuthResult> GenerateUserTokenAsync(User user)
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
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

                },
                expires: DateTime.UtcNow.Add(TimeSpan.Parse("00:01")),
                signingCredentials: signInCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);

            var refreshToken = new RefreshToken
            {
                JwtId = tokenOption.Id,
                Token = RandomStringGenerator(23),
                ExpieryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked = false,
                IsUsed = false,
                UserUUID = user.UUID
            };
            await _RefreshTokenRepository.AddEntity(refreshToken);
            await _RefreshTokenRepository.SaveEntity();
            return new AuthResult
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                Result = true
            };
        }

        public async Task<AuthResult> GenerateAdminToken(Admin admin)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Wx7Xl6rPABrWvLbLaXoBcaLQ8nQJg7L1Dce3zfE0"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            //get all user claims
            var claims = await AllValidClaims(admin);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = PathTools.Domain,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse("00:01")),
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)

            };

            var Token = JwtTokenHandler.CreateToken(tokenDescriptor);

            var JwtToken =new JwtSecurityTokenHandler().WriteToken(Token);

            var refreshToken = new RefreshToken
            {
                JwtId = Token.Id,
                Token = RandomStringGenerator(23),
                ExpieryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked=false,
                IsUsed=false,
                UserUUID=admin.UUID
            };
            await _RefreshTokenRepository.AddEntity(refreshToken);
            await _RefreshTokenRepository.SaveEntity();

            return new AuthResult
            {
                Token = JwtToken,
                RefreshToken = refreshToken.Token,
                Result = true
            };
        }

        private async Task<List<Claim>> AllValidClaims(Admin admin)
        {
            var _options = new IdentityOptions();

            var claims = new List<Claim>()
            {
                    new Claim(ClaimTypes.NameIdentifier,admin.UUID.ToString()),
                    new Claim(ClaimTypes.Sid,admin.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,admin.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
            };
            var userRoles = await _roleService.GetAdminRole(admin.Id);

            if (userRoles != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoles.Name));
                var permissions = await _roleService.GetSelectedPermissionOfRole(userRoles.Id);
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim(permission.PermissionName, "true"));
                }
            }

            return claims;
        }

        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

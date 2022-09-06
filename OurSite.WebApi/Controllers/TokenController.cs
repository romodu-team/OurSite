using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        #region constructor
        private TokenValidationParameters _tokenValidationParams;
        private IGenericRepository<RefreshToken> _RefreshTokenRepository;
        private IGenericRepository<User> _UserRepository;
        private IGenericRepository<Admin> _AdminRepository;
        private IRoleService _roleService;

        public TokenController(IRoleService RoleService, TokenValidationParameters tokenValidationParams, IGenericRepository<RefreshToken> RefreshTokenRepository, IGenericRepository<User> UserRepository, IGenericRepository<Admin> AdminRepository)
        {
            _tokenValidationParams = tokenValidationParams;
            _RefreshTokenRepository = RefreshTokenRepository;
            _UserRepository = UserRepository;
            _AdminRepository = AdminRepository;
            _roleService = RoleService;
        }


        #endregion
        /// <summary>
        /// If the user's jwt token expires (it will expire every 1 minute), use this controller to get a new jwt token.
        ///If the user's refresh token is still valid, you will receive the new jwt token.
        ///Otherwise, you will receive "Invalid tokens" or "Expired tokens" errors that the user must login again.
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenRequest);

                if (result == null)
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                    {
                        "Invalid tokens"
                    },
                        Result = false
                    });

                return Ok(result);
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
            {
                "Invalid parameters"
            },
                Result = false
            });
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParams.ValidateLifetime = true; // for testing

                var tokenInVerification =
                    jwtTokenHandler.ReadJwtToken(tokenRequest.Token);
                
                if (tokenInVerification is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                        return null;
                }

                //var utcExpiryDate = long.Parse(tokenInVerification.Claims
                //    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);


                //var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                //if (expiryDate < DateTime.UtcNow)
                //{
                //    return new AuthResult()
                //    {
                //        Result = false,
                //        Errors = new List<string>()
                //    {
                //        "Expired token"
                //    }
                //    };
                //}

                var storedToken = await _RefreshTokenRepository.GetAllEntity().FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                    return new AuthResult() //invalid refresh token
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "Invalid tokens"
                    }
                    };

                if (storedToken.IsUsed)
                    return new AuthResult() //invalid refresh token
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "Invalid tokens"
                    }
                    };

                if (storedToken.IsRevoked) //invalid refresh token
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "Invalid tokens"
                    }
                    };

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti) //jwt token and refresh token does not match
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "Invalid tokens"
                    }
                    };

                if (storedToken.ExpieryDate < DateTime.UtcNow)//refresh token expired , login again
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                    {
                        "Expired tokens"
                    }
                    };


                storedToken.IsUsed = true;
                _RefreshTokenRepository.UpDateEntity(storedToken);
                await _RefreshTokenRepository.SaveEntity();
                AuthenticationHelper authenticationHelper = new AuthenticationHelper(_roleService, _RefreshTokenRepository);
                var dbUser = await _UserRepository.GetAllEntity().SingleOrDefaultAsync(u=>u.UUID==storedToken.UserUUID && u.IsRemove==false);
                if(dbUser != null)
                    return await authenticationHelper.GenerateUserTokenAsync(dbUser);
                else
                {
                    var dbAdmin = await _AdminRepository.GetAllEntity().SingleOrDefaultAsync(u => u.UUID == storedToken.UserUUID && u.IsRemove == false);
                    if( dbAdmin != null )
                        return await authenticationHelper.GenerateAdminToken(dbAdmin);

                }
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                {
                    "User not found"
                }
                };

            }
            catch (Exception e)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                {
                    "Server error"
                }
                };
            }
        }
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
    }
}

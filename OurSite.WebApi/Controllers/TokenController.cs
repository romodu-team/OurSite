using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.Core.Utilities.Extentions;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Wangkanai.Detection.Services;

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
        private readonly IConfiguration _configuration;
        private readonly IDetectionService detectionService;
        public TokenController(IDetectionService detectionService, IConfiguration configuration, IRoleService RoleService, TokenValidationParameters tokenValidationParams, IGenericRepository<RefreshToken> RefreshTokenRepository, IGenericRepository<User> UserRepository, IGenericRepository<Admin> AdminRepository)
        {
            _tokenValidationParams = tokenValidationParams;
            _RefreshTokenRepository = RefreshTokenRepository;
            _UserRepository = UserRepository;
            _AdminRepository = AdminRepository;
            _roleService = RoleService;
            _configuration = configuration;
            this.detectionService = detectionService;
        }


        #endregion
        /// <summary>
        /// If the user's jwt token expires (it will expire every 1 minute), use this controller to get a new jwt token and new refreshToken.
        ///If the user's refresh token is still valid, you will receive the new jwt token and new refreshToken.
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
                AuthenticationHelper authenticationHelper = new AuthenticationHelper(_roleService, _RefreshTokenRepository,detectionService);
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
        /// <summary>
        /// View the list of active user sessions
        /// </summary>
        /// <param name="AccountUUID"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpGet("View-All-Session")]
        [Authorize]
        public async Task<IActionResult> ViewAllSession([FromQuery] Guid AccountUUID,[FromQuery]string refreshToken)
        {
            var session = await _RefreshTokenRepository.GetAllEntity().Where(r => r.UserUUID == AccountUUID && r.IsRevoked == false && r.IsRemove == false && r.ExpieryDate>=DateTime.UtcNow).Select(x=> new ViewAllSessionDto { Id=x.Id,Browser=x.UserBrowser,Platform=x.UserPlatform,LoginDate=x.CreateDate.PersianDate(),IsCurrentSession=x.Token==refreshToken}).ToListAsync();
            return Ok(session);

        }

        /// <summary>
        /// revoke a session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpPost("Revoke-session")]
        [Authorize]
        public async Task<IActionResult> RevokeSession([FromQuery] long sessionId)
        {
            var sessionToken = await _RefreshTokenRepository.GetAllEntity().SingleOrDefaultAsync(x => x.Id == sessionId && x.IsRemove == false);
            if(sessionToken is not null){
                if (sessionToken.IsRevoked)
                {
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.Error("It has already been revoked");

                }
                else
                {
                    sessionToken.IsRevoked = true;
                    try
                    {
                        _RefreshTokenRepository.UpDateEntity(sessionToken);
                        await _RefreshTokenRepository.SaveEntity();
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success("success");
                    }
                    catch (Exception)
                    {
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("server error");
                    }
                   
                }
            }
            HttpContext.Response.StatusCode = 404;

            return JsonStatusResponse.NotFound("session not found");
        }
        /// <summary>
        /// revoke all user sessions except current session 
        /// </summary>
        /// <param name="AccountUUID"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("revoke-all-sessions")]
        [Authorize]
        public async Task<IActionResult> RevokeAllSessions([FromQuery] Guid AccountUUID, [FromQuery] string refreshToken)
        {
            var sessions = await _RefreshTokenRepository.GetAllEntity().Where(x => x.UserUUID == AccountUUID && x.IsRemove == false && x.IsRevoked == false && x.Token != refreshToken).ToListAsync();
            foreach (var session in sessions)
            {
                 session.IsRevoked = true;
                _RefreshTokenRepository.UpDateEntity(session);
            }
            try
            {
                await _RefreshTokenRepository.SaveEntity();
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("success");
            }
            catch (Exception)
            {

                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("server error");
            }
        }
        /// <summary>
        /// !important :call this method for revoke session from database , The frontend still needs to remove the token from the cookie or...
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("log-out")]
        public async Task<IActionResult> LogOut([FromBody] string refreshToken)
        {
            var session = await _RefreshTokenRepository.GetAllEntity().SingleOrDefaultAsync(x=>x.Token==refreshToken && x.IsRemove==false && x.IsRevoked == false);
            if(session is not null)
            {
                session.IsRevoked = true;
                try
                {
                    _RefreshTokenRepository.UpDateEntity(session);
                    await _RefreshTokenRepository.SaveEntity();
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("success");
                }
                catch (Exception)
                {

                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                }
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("session not found");
        }

    }
}

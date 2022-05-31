using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("login-Admin")]
        public Task<IActionResult> LoginAdmin(ReqLoginDto reqLogin)
        {

        }
    }
}

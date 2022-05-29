using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService userservice;
        public UserController(IUserService userService)
        {
            this.userservice = userservice;
        }

        
    }
}


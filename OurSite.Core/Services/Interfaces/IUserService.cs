using OurSite.Core.DTOs;
using OurSite.DataLayer.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface IUserService:IDisposable
    {
        Task<ResLoginDto> LoginUser(ReqLoginUserDto login);
        Task<bool> IsUserActiveByUserName(string userName);
        Task<User> GetUserByUserPass(string username,string password);
        
    }
}

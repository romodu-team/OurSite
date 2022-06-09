using OurSite.Core.DTOs;
using OurSite.DataLayer.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.Core.DTOs.UserDtos;

namespace OurSite.Core.Services.Interfaces
{
    public interface IUserService:IDisposable
    {
        Task<bool> IsUserActiveByUserName(string userName);
        Task<User> GetUserByUserPass(string username,string password);
        Task<ResLoginDto> LoginUser(ReqLoginDto login);
        Task<bool> ResetPassword(ReqResetPassword request);
        Task<User> GetUserByEmailOrUserName(string EmailOrUserName);
        Task<ResLoginDto> SendResetPassEmail(string EmailOrUserName);
        Task<ResActiveUser> ActiveUser(string activationCode);
        Task<bool> GetUserEmailandUserName(string Email , string UserName);

        Task<bool> UpDate(ReqUpdateUserDto userdto);
        Task<RessingupDto> SingUp(ReqSingupUserDto userDto);

        Task<ReqViewUserDto> ViewProfile(long id);
        Task<User> ViewUser(long id);
        Task<bool> ChangeUserStatus(long userId);
        Task<List<User>> GetAlluser();
        Task<bool> DeleteUser(long id);
        Task AddUser(ReqSingupUserDto userDto);


    }
}

using OurSite.Core.DTOs;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.DTOs.AdminDtos;

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

        Task<ResUpdate> UpDate(ReqUpdateUserDto userdto,long id);
        Task<bool> IsMobileExist(string mobile , accountType type);
        Task<RessingupDto> SingUp(ReqSingupUserDto userDto);

        Task<ReqViewUserDto> ViewProfile(long id);
        Task<ResViewuserAdminDto> ViewUser(long id);
        Task<bool> ChangeUserStatus(long userId);

        #region User Mangement by admin
        Task<List<GetAllUserDto>> GetAlluser();
        Task<bool> DeleteUser(long id);
        Task<ResadduserDto> AddUser(ReqAddUserAdminDto userDto);
        #endregion





    }
}

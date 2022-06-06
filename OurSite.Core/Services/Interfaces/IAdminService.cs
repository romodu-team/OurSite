using System;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;

namespace OurSite.Core.Services.Interfaces
{
    public interface IAdminService : IDisposable
    {
        #region Admin management
        Task<Role> GetAdminRole(long adminId);
        Task<bool> DeleteAdmin(long adminId);
        Task<resUpdateAdmin> UpdateAdmin(ReqUpdateAdminDto req);

        Task<ResViewAdminDto> GetAdminById(long adminId);
        Task<RessingupDto> RegisterAdmin(ReqSingupUserDto req);

        Task<bool> IsAdminExist(string UserName, string Email);
        #endregion

        #region User Management
        Task GetAlluser();
        Task<bool> DeleteUser(long id);
        Task AddUser(ReqSingupUserDto userDto);
        #endregion

        #region Login
        Task<Admin> Login(ReqLoginDto req);
        #endregion



    }
}


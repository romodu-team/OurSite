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
		Task<ResViewuserAdminDto> ViewUser(string find);
		Task GetAlluser();
		Task<bool> DeleteUser(long id);
		Task AddUser(ReqSingupUserDto userDto);

		Task<Admin> Login(ReqLoginDto req);

		Task<Role> GetAdminRole(long adminId);
		Task<bool> DeleteAdmin(long adminId);
		Task<resUpdateAdmin> UpdateAdmin(ReqUpdateAdminDto req);

		Task<ResViewAdminDto> GetAdmin(long adminId);
		
	}
}


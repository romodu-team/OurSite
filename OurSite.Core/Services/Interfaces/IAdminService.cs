using System;
using OurSite.Core.DTOs;
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

	//	Task<Admin> GetAdminByUserPass(ReqLoginDto req);
		Task<Role> GetAdminRole(long adminId);
	}
}


using System;
using OurSite.Core.DTOs;

namespace OurSite.Core.Services.Interfaces
{
	public interface IAdminService : IDisposable
	{
		Task<ResViewuserAdminDto> ViewUser(string find);
		Task GetAlluser();
		Task<bool> DeleteUser(long id);
		Task AddUser(ReqSingupUserDto userDto);

	}
}


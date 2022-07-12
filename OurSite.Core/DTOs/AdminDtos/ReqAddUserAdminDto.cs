using System;
namespace OurSite.Core.DTOs.AdminDtos
{
	public class ReqAddUserAdminDto
	{
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public enum ResadduserDto
    {
        success,
        Failed,
        Exist
    };
}


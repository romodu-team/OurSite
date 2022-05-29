using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.Core.DTOs;

namespace OurSite.Core.Services.Interfaces
{
    public interface IUserService:IDisposable
    {

        Task<singup> SingUp(ReqSingupUserDto userDto);
        Task UpDate(ReqUpdateUserDto userdto);
        Task ViewProfile(long id);


    }
}

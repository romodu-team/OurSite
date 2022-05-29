﻿using OurSite.Core.DTOs;
using OurSite.DataLayer.Entities.Accounts;
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
        Task<bool> IsUserActiveByUserName(string userName);
        Task<User> GetUserByUserPass(string username,string password);
        

        Task<singup> SingUp(ReqSingupUserDto userDto);
        Task UpDate(ReqUpdateUserDto userdto);
        Task ViewProfile(long id);


    }
}

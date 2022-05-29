﻿using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IGenericReopsitories<User> userService;
        public UserService(IGenericReopsitories<User> userService)
        {
            this.userService = userService;
      
        }
        #endregion

        #region Login
        public async Task<ResLoginDto> LoginUser(ReqLoginUserDto login)
        {
            var user = await GetUserByUserPass(login.UserName, login.Password);
            if (user != null)
            {
                if (await IsUserActiveByUserName(login.UserName))
                {
                    return ResLoginDto.Success;
                }
                return ResLoginDto.NotActived;
            }
            else
                return ResLoginDto.IncorrectData;
            
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
        }

        public async Task<bool> IsUserActiveByUserName(string userName)
        {
            return await userService.GetAllEntity().AnyAsync(u => u.Email == userName && u.IsActive==true);
        }

        public async Task<User> GetUserByUserPass(string username, string password)
        {
            var user =await userService.GetAllEntity().SingleOrDefaultAsync(u => u.UserName == username && u.Password == password);
            return user;
        }


        #endregion

    }
}

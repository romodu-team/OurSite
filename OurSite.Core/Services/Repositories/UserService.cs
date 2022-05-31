
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
﻿using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
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
        private IPasswordHelper passwordHelper;
        private IMailService mailService;
        public UserService(IGenericReopsitories<User> userService, IPasswordHelper passwordHelper, IMailService mailService)
        {
            this.userService = userService;
            this.passwordHelper = passwordHelper;
            this.mailService = mailService;
        }
        #endregion

        #region Login
        public async Task<ResLoginDto> LoginUser(ReqLoginDto login)
        {
            login.Password = passwordHelper.EncodePasswordMd5(login.Password);
            var user = await GetUserByUserPass(login.UserName, login.Password);
            if (user != null)
            {
                if (await IsUserActiveByUserName(login.UserName.ToLower().Trim()))
                {
                    return ResLoginDto.Success;
                }
                return ResLoginDto.NotActived;
            }
            else
                return ResLoginDto.IncorrectData;
            
        }
        #endregion

        #region forgot password
        public async Task<bool> ForgotPassword(ReqForgotPassword request)
        {
            try
            {
                var user = await userService.GetEntity(request.UserId);
                user.Password = passwordHelper.EncodePasswordMd5(request.Password);
                userService.UpDateEntity(user);
                await userService.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        #endregion

        #region Rest Password
        public async Task<ResLoginDto> SendResetPassEmail(string EmailOrUserName)
        {
            var user =await GetUserByEmailOrUserName(EmailOrUserName.ToLower().Trim());
            if (user != null)
            {
              var res= await mailService.SendResetPasswordEmailAsync(new SendEmailDto { Parameter=user.Id.ToString() ,ToEmail=user.Email,UserName=user.UserName});
                if (res)
                    return ResLoginDto.Success;
                else
                    return ResLoginDto.Error;
            }
            return ResLoginDto.IncorrectData;

        }
        #endregion

        #region Get User by email and username
        public async Task<User> GetUserByEmailOrUserName(string EmailOrUserName)
        {
            return await userService.GetAllEntity().SingleOrDefaultAsync(u => u.Email == EmailOrUserName.ToLower().Trim() || u.UserName == EmailOrUserName.ToLower().Trim() && u.IsRemove == false);
        }

        #endregion

        #region Active user by username
        public async Task<bool> IsUserActiveByUserName(string userName)
        {
            return await userService.GetAllEntity().Where(u => u.UserName == userName.ToLower().Trim()).AnyAsync(x => x.IsActive == true);
        }

        #endregion

        #region Get user by password
        public async Task<User> GetUserByUserPass(string username, string password)
        {
            var user = await userService.GetAllEntity().SingleOrDefaultAsync(u => u.UserName == username.ToLower().Trim() && u.Password == password && u.IsRemove == false);
            return user;
        }

        #endregion

        #region Active User
        public async Task<ResActiveUser> ActiveUser(string activationCode)
        {
            var user = await userService.GetAllEntity().SingleOrDefaultAsync(u => u.IsActive == false && u.ActivationCode == activationCode && u.IsRemove == false);
            if (user != null)
            {
                user.IsActive = true;
                user.ActivationCode = new Guid().ToString();
                try
                {
                    userService.UpDateEntity(user);
                    await userService.SaveEntity();
                    return ResActiveUser.Success;
                }
                catch (Exception)
                {

                    return ResActiveUser.Failed;
                }

            }
            return ResActiveUser.Failed;
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
        }


        #endregion

        #region SingUp exist error
        public async Task<bool> GetUserEmailandUserName(string Email, string UserName)
        {
            return await userService.GetAllEntity().AnyAsync(x => x.Email == Email || x.UserName == UserName);

        }

        #endregion

        #region singup
        public async Task<singup> SingUp(ReqSingupUserDto userDto)
        {
            var check = await GetUserEmailandUserName(userDto.Email, userDto.UserName);
            if (check == true)
               return singup.Exist;
            
            try
            {
                User user = new User()
                {
                    FirstName = userDto.Name,
                    LastName = userDto.Family,
                    UserName = userDto.UserName,
                    Password = userDto.Password,
                    Email = userDto.Email,
                    Phone = userDto.phone,
                    ActivationCode = new Guid().ToString()
                    

                };
                await userService.AddEntity(user);
                await userService.SaveEntity();
                await mailService.SendActivationCodeEmail(new SendEmailDto { ToEmail = userDto.Email, UserName = userDto.UserName, Parameter = user.ActivationCode});

                return singup.success;

            }
            catch (Exception ex)
            {
               return singup.Failed;

            }


            
        }
        #endregion

        #region Update profile by user
        public async Task UpDate(ReqUpdateUserDto userdto)
        {
            User user = new User()
            {
                FirstName = userdto.FirstName,
                LastName = userdto.LastName,
                NationalCode = userdto.NationalCode,
                Email = userdto.Email,
                Mobile = userdto.Mobile,
                Password = userdto.Password,
                Gender = (DataLayer.Entities.BaseEntities.gender?)userdto.Gender,
                Address = userdto.Address,
                ImageName = userdto.ImageName,
                Birthday = userdto.Birthday,
                Phone = userdto.Phone,
                ShabaNumbers = userdto.ShabaNumbers,
                AccountType = (DataLayer.Entities.Accounts.accountType)userdto.AccountType,
                BusinessCode = userdto.BusinessCode,
                RegistrationNumber = userdto.RegistrationNumber
            };

             userService.UpDateEntity(user);
            await userService.SaveEntity();


        }
        #endregion

        #region view profile by user
        public async Task<User> ViewProfile(long id)
        {
          var user=  await userService.GetEntity(id);
            return user;


        }
        #endregion

    }
}

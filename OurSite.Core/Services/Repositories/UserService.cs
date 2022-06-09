
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
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.DTOs.MailDtos;
using Microsoft.AspNetCore.Authorization;

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
                user.LastUpdate = DateTime.Now;
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

        #region Send Rest Password Email
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

        #region Get User by email Or username
        public async Task<User> GetUserByEmailOrUserName(string EmailOrUserName)
        {
            return await userService.GetAllEntity().SingleOrDefaultAsync(u => u.Email == EmailOrUserName.ToLower().Trim() || u.UserName == EmailOrUserName.ToLower().Trim() && u.IsRemove == false);
        }

        #endregion

        #region Check user activation by username
        public async Task<bool> IsUserActiveByUserName(string userName)
        {
            return await userService.GetAllEntity().AnyAsync(u=>(u.UserName==userName.ToLower().Trim()|| u.Email == userName.ToLower().Trim())&& u.IsActive==true);
        }

        #endregion

        #region Get user by Username and password
        public async Task<User> GetUserByUserPass(string username, string password)
        {
            var user = await userService.GetAllEntity().SingleOrDefaultAsync(u => (u.UserName == username.ToLower().Trim()|| u.Email== username.ToLower().Trim()) && u.Password == password && u.IsRemove==false);
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
                    user.LastUpdate = DateTime.Now;
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
        public async Task<RessingupDto> SingUp(ReqSingupUserDto userDto)
        {
            var check = await GetUserEmailandUserName(userDto.Email, userDto.UserName);
            if (check == true)
               return RessingupDto.Exist;
            
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
                user.CreateDate = DateTime.Now;
                user.LastUpdate = user.CreateDate;
                await userService.AddEntity(user);
                await userService.SaveEntity();
                await mailService.SendActivationCodeEmail(new SendEmailDto { ToEmail = userDto.Email, UserName = userDto.UserName, Parameter = user.ActivationCode});

                return RessingupDto.success;

            }
            catch (Exception ex)
            {
               return RessingupDto.Failed;

            }


            
        }
        #endregion

        #region Update profile by user
        public async Task<bool> UpDate(ReqUpdateUserDto userdto)
        {
            var user = await userService.GetEntity(userdto.id);
            if (user != null)
            {
                user.FirstName = userdto.FirstName;
                user.LastName = userdto.LastName;
                user.NationalCode = userdto.NationalCode;
                user.Email = userdto.Email;
                user.Mobile = userdto.Mobile;
                user.Password = userdto.Password;
                user.Gender = (DataLayer.Entities.BaseEntities.gender?)userdto.Gender;
                user.Address = userdto.Address;
                user.ImageName = userdto.ImageName;
                user.Birthday = userdto.Birthday;
                user.Phone = userdto.Phone;
                user.ShabaNumbers = userdto.ShabaNumbers;
                user.AccountType = (DataLayer.Entities.Accounts.accountType)userdto.AccountType;
                user.BusinessCode = userdto.BusinessCode;
                user.RegistrationNumber = userdto.RegistrationNumber;
                try
                {

                    userService.UpDateEntity(user);
                    await userService.SaveEntity();
                    return true;

                }
                catch (Exception ex)
                {
                    return false;


                }

            }
            return false;



        }
        #endregion

        #region view profile by user
        public async Task<ReqViewUserDto> ViewProfile(long id)
        {
           var user = await userService.GetEntity(id);
            ReqViewUserDto userdto = new ReqViewUserDto();
            if (user is not null)
            {
                userdto.UserName = user.UserName;
                userdto.FirstName = user.FirstName;
                userdto.LastName = user.LastName;
                userdto.NationalCode = user.NationalCode;
                userdto.Email = user.Email;
                userdto.Mobile = user.Mobile;
                userdto.Gender = (gender?)user.Gender;
                userdto.Address = user.Address;
                userdto.ImageName = user.ImageName;
                userdto.Phone = user.Phone;
                userdto.Birthday = user.Birthday;
                userdto.ShabaNumbers = user.ShabaNumbers;
                userdto.AccountType = (DTOs.UserDtos.accountType?)user.AccountType;
                userdto.BusinessCode = user.BusinessCode;
                userdto.RegistrationNumber = user.RegistrationNumber;

                return userdto;
            }
            return null;
                
        }

        #endregion

        #region Change user status
        public async Task<bool> ChangeUserStatus(long userId)
        {
            try
            {
                var user = await userService.GetEntity(userId);
                user.IsActive = !user.IsActive;
                user.LastUpdate = DateTime.Now;
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

        #region view user by admin
        //profile view for admin
        [Authorize(Roles = "نقش مدنظر")]
        public async Task<User> ViewUser(long id)
        {
            var user = await userService.GetEntity(id);
            return user;

        }
        #endregion


        #region Get user list
        public Task<List<User>> GetAlluser() //for return a list of user that singup in our site for admin
        {
            var list = userService.GetAllEntity().Where(u => u.IsRemove == false).ToListAsync();    //use the genric interface options and save values in variable
            return list;
        }
        #endregion

    }
}

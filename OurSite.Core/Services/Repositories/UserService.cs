using Microsoft.EntityFrameworkCore;
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
        public async Task<User> GetUserByEmailOrUserName(string EmailOrUserName)
        {
            return await userService.GetAllEntity().SingleOrDefaultAsync(u => u.Email == EmailOrUserName.ToLower().Trim() || u.UserName == EmailOrUserName.ToLower().Trim() && u.IsRemove==false);
        }

        public async Task<bool> IsUserActiveByUserName(string userName)
        {
            return await userService.GetAllEntity().Where(u=>u.UserName==userName.ToLower().Trim()).AnyAsync(x=> x.IsActive == true);
        }

        public async Task<User> GetUserByUserPass(string username, string password)
        {
            var user = await userService.GetAllEntity().SingleOrDefaultAsync(u => u.UserName == username.ToLower().Trim() && u.Password == password && u.IsRemove==false);
            return user;
        }

        public async Task<ResActiveUser> ActiveUser(string activationCode)
        {
            var user =await userService.GetAllEntity().SingleOrDefaultAsync(u => u.IsActive == false && u.ActivationCode == activationCode && u.IsRemove == false);
            if (user !=null)
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
        public async Task<singup> SingUp(ReqSingupUserDto userDto)
        {
            throw new NotImplementedException();
            //if signup is success , send email activation code
            await mailService.SendActivationCodeEmail(new SendEmailDto { ToEmail=userDto.Email,UserName=userDto.UserName,Parameter="کد فعالسازی ساخته شده اینجا قرار گیرد"});
        }

        public Task UpDate(ReqUpdateUserDto userdto)
        {
            throw new NotImplementedException();
        }

        public Task ViewProfile(long id)
        {
            throw new NotImplementedException();
        }


        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
        }


        #endregion

    }
}

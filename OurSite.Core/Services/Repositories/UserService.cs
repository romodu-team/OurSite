
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
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
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.DTOs.MailDtos;
using Microsoft.AspNetCore.Authorization;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.Uploader;
using Microsoft.AspNetCore.Http;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.Core.DTOs.Paging;

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

        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
        }


        #endregion

        #region User Service

        #region singup
        public async Task<RessingupDto> SingUp(ReqSingupUserDto userDto)
        {
            userDto.UserName = userDto.UserName.ToLower().Trim();
            userDto.Email = userDto.Email.ToLower().Trim();
            userDto.Mobile = userDto.Mobile.Trim();
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
                    Password = passwordHelper.EncodePasswordMd5(userDto.Password),
                    Email = userDto.Email,
                    Mobile = userDto.Mobile,
                    ActivationCode = Guid.NewGuid().ToString()


                };
                user.CreateDate = DateTime.Now;
                user.LastUpdate = user.CreateDate;
                await userService.AddEntity(user);
                await userService.SaveEntity();
                await mailService.SendActivationCodeEmail(new SendEmailDto { ToEmail = userDto.Email, UserName = userDto.UserName, Parameter = user.ActivationCode });

                return RessingupDto.success;

            }
            catch (Exception ex)
            {
                return RessingupDto.Failed;

            }



        }
        #endregion

        #region SingUp exist error
        public async Task<bool> GetUserEmailandUserName(string? Email, string UserName)
        {
            return await userService.GetAllEntity().AnyAsync(x =>!x.IsRemove &&(x.Email == Email.Trim().ToLower() || x.UserName == UserName.Trim().ToLower()));

        }

        #endregion

        #region Login
        public async Task<ResLoginDto> LoginUser(ReqLoginDto login)
        {
            login.UserName = login.UserName.Trim().ToLower();
            if (String.IsNullOrWhiteSpace(login.UserName))
                return ResLoginDto.Error;
            if (String.IsNullOrWhiteSpace(login.Password))
                return ResLoginDto.Error;

            login.Password = passwordHelper.EncodePasswordMd5(login.Password);
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

        #region Reset password
        public async Task<bool> ResetPassword(ReqResetPassword request)
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
            var user = await GetUserByEmailOrUserName(EmailOrUserName.ToLower().Trim());
            if (user != null)
            {
                var res = await mailService.SendResetPasswordEmailAsync(new SendEmailDto { Parameter = user.Id.ToString(), ToEmail = user.Email.Trim().ToLower(), UserName = user.UserName.Trim().ToLower() });
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
            return await userService.GetAllEntity().AnyAsync(u => (u.UserName == userName.ToLower().Trim() || u.Email == userName.ToLower().Trim()) && u.IsActive == true && u.IsRemove==false);
        }

        #endregion

        #region Get user by Username and password
        public async Task<User> GetUserByUserPass(string username, string password)
        {
            var user = await userService.GetAllEntity().SingleOrDefaultAsync(u => (u.UserName == username.ToLower().Trim() || u.Email == username.ToLower().Trim()) && u.Password == password && u.IsRemove == false);
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
                user.ActivationCode = Guid.NewGuid().ToString();
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
            return ResActiveUser.NotFoundOrActivated;
        }

        #endregion

        #region Update profile by user
        public async Task<bool> UpDate(ReqUpdateUserDto userdto)
        {
            var user = await userService.GetEntity(userdto.id);
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(userdto.FirstName))
                    user.FirstName = userdto.FirstName;
                if (!string.IsNullOrWhiteSpace(userdto.LastName))
                    user.LastName = userdto.LastName;
                if (!string.IsNullOrWhiteSpace(userdto.NationalCode))
                    user.NationalCode = userdto.NationalCode;
                if (!string.IsNullOrWhiteSpace(userdto.Email))
                    user.Email = userdto.Email;
                if (!string.IsNullOrWhiteSpace(userdto.Mobile))
                    user.Mobile = userdto.Mobile;
                if (!string.IsNullOrWhiteSpace(userdto.Password))
                    user.Password = userdto.Password;
                if (userdto.Gender is not null)
                    user.Gender = (DataLayer.Entities.BaseEntities.gender?)userdto.Gender;
                if (!string.IsNullOrWhiteSpace(userdto.Address))
                    user.Address = userdto.Address;
                if (!string.IsNullOrWhiteSpace(userdto.ImageName))
                    user.ImageName = userdto.ImageName;
                if (!string.IsNullOrWhiteSpace(userdto.Birthday))
                    user.Birthday = userdto.Birthday;
                if (!string.IsNullOrWhiteSpace(userdto.Phone))
                    user.Phone = userdto.Phone;
                if (!string.IsNullOrWhiteSpace(userdto.ShabaNumbers))
                    user.ShabaNumbers = userdto.ShabaNumbers;
                if (userdto.AccountType is not null)
                    user.AccountType = (DataLayer.Entities.Accounts.accountType)userdto.AccountType;
                if (!string.IsNullOrWhiteSpace(userdto.BusinessCode))
                    user.BusinessCode = userdto.BusinessCode;
                if (!string.IsNullOrWhiteSpace(userdto.RegistrationNumber))
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

        public async Task<resFileUploader> ProfilePhotoUpload(IFormFile photo,long UserId)
        {
            var result =await FileUploader.UploadFile(PathTools.ProfilePhotos, photo,3);
            if (result.Status==resFileUploader.Success)
            {
               User user= await userService.GetEntity(UserId);
               user.ImageName = result.FileName;
               userService.UpDateEntity(user);
               await userService.SaveEntity();
            }
            return result.Status;
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

        #endregion

        #region User Management By Admin



        #region Delete User
        public async Task<bool> DeleteUser(long id)
        {
            var user = await userService.DeleteEntity(id); //get id and return true that it means user deleted
            if (user is true) //if delete
            {
                userService.SaveEntity();
                return true;
               
            }
            return false; //if not

        }
        #endregion

        #region Add User
        //add user by admin
        //[Authorize(Roles = "نقش مدنظر")]
        public async Task<ResadduserDto> AddUser(ReqAddUserAdminDto userDto)
        {
            if(!string.IsNullOrWhiteSpace(userDto.UserName) && !string.IsNullOrWhiteSpace(userDto.Password))
            {
                if (!await GetUserEmailandUserName(null,userDto.UserName))
                {
                    User user = new User()
                    {
                        UserName = userDto.UserName,
                        Password = userDto.Password

                    };
                    await userService.AddEntity(user);
                    await userService.SaveEntity();
                    return ResadduserDto.success;

                }
                return ResadduserDto.Exist;
            }

            return ResadduserDto.Failed;
        }
        #endregion

        #region Get user list 
        public async Task<List<GetAllUserDto>> GetAlluser(FilterUserDto filter) //for return a list of user that singup in our site for admin
        {
            var usersQuery = userService.GetAllEntity();
            var count = (int)Math.Ceiling(usersQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list =await userService.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(u=> new GetAllUserDto { Email=u.Email,FirstName=u.FirstName,LastName=u.LastName,IsActive=u.IsActive,UserId=u.Id,UserName=u.UserName,IsDelete=u.IsRemove}).ToListAsync();    //use the genric interface options and save values in variable
            return list;
        }

        #endregion

        #region View User Profile
        //profile view for admin
        [Authorize(Roles = "نقش مدنظر")]
        public async Task<ResViewuserAdminDto> ViewUser(long id)
        {
            var user = await userService.GetEntity(id);
            ResViewuserAdminDto adminview = new ResViewuserAdminDto();
            if (user is not null)
            {
                adminview.Id = user.Id;
                adminview.CreateDate = user.CreateDate;
                adminview.IsRemove = user.IsRemove;
                adminview.LastUpdate = user.LastUpdate;
                adminview.UserName = user.UserName;
                adminview.FirstName = user.FirstName;
                adminview.LastName = user.LastName;
                adminview.NationalCode = user.NationalCode;
                adminview.Email = user.Email;
                adminview.Mobile = user.Mobile;
                adminview.Gender = (gender?)user.Gender;
                adminview.Address = user.Address;
                adminview.ImageName = user.ImageName;
                adminview.Phone = user.Phone;
                adminview.Birthday = user.Birthday;
                adminview.ShabaNumbers = user.ShabaNumbers;
                adminview.AccountType = (Core.DTOs.UserDtos.accountType?)user.AccountType;
                adminview.BusinessCode = user.BusinessCode;
                adminview.RegistrationNumber = user.RegistrationNumber;
            }
            return adminview;
        }
        #endregion

        #endregion


    }
}
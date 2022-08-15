
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
using gender = OurSite.DataLayer.Entities.Accounts.gender;

namespace OurSite.Core.Services.Repositories
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IGenericReopsitories<User> userService;
        private readonly IGenericReopsitories<AdditionalDataOfUser> additionalDataRepository;
        private IPasswordHelper passwordHelper;
        private IMailService mailService;
        public UserService(IGenericReopsitories<AdditionalDataOfUser> additionalDataRepository, IGenericReopsitories<User> userService, IPasswordHelper passwordHelper, IMailService mailService)
        {
            this.userService = userService;
            this.passwordHelper = passwordHelper;
            this.mailService = mailService;
            this.additionalDataRepository = additionalDataRepository;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
            additionalDataRepository.Dispose();
            
        }


        #endregion
        public async Task<bool> IsMobileExist(string mobile, accountType type)
        {
            return userService.GetAllEntity().Any(x => x.Mobile == mobile && x.AccountType == type);
        }
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
          
            if (userDto.AccountType == accountType.Legal)
            {
                if (await IsMobileExist(userDto.Mobile, userDto.AccountType)) return RessingupDto.MobileExist;

            }
            if(userDto.AccountType == accountType.Real)
            {
                if (await IsMobileExist(userDto.Mobile, userDto.AccountType)) return RessingupDto.MobileExist;
            }
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
                    ActivationCode = Guid.NewGuid().ToString(),
                    AccountType = userDto.AccountType

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
        public async Task<bool> GetUserEmailandUserName( string UserName)
        {
            return await userService.GetAllEntity().AnyAsync(x => !x.IsRemove && (x.UserName == UserName.Trim().ToLower()));

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
        public async Task<bool> IsUserExist(long userId)
        {
            return await userService.GetAllEntity().AnyAsync(u => u.Id == userId && u.IsRemove==false && u.IsActive==true);
        }
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
        public async Task<ResUpdate> UpDate(ReqUpdateUserDto userdto,long id)
        {
            var user = await userService.GetAllEntity().Include(u => u.AdditionalDataOfUser).SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(userdto.FirstName))
                    user.FirstName = userdto.FirstName;
                if (!string.IsNullOrWhiteSpace(userdto.LastName))
                    user.LastName = userdto.LastName;
                if (!string.IsNullOrWhiteSpace(userdto.NationalCode))
                    user.NationalCode = userdto.NationalCode;
                if (!string.IsNullOrWhiteSpace(userdto.Email))
                    user.Email = userdto.Email.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(userdto.Mobile))
                    user.Mobile = userdto.Mobile;
                
                if (!string.IsNullOrWhiteSpace(userdto.ImageName))
                    user.ImageName = userdto.ImageName;
               
                if (userdto.AccountType is not null)
                    user.AccountType = (DataLayer.Entities.Accounts.accountType)userdto.AccountType;
                

                if (user.AdditionalDataOfUser is not null)
                {
                    if (userdto.Gender is not null)
                        user.AdditionalDataOfUser.Gender = (DataLayer.Entities.Accounts.gender?)userdto.Gender;
                    if (!string.IsNullOrWhiteSpace(userdto.Address))
                        user.AdditionalDataOfUser.Address = userdto.Address;
                    if (!string.IsNullOrWhiteSpace(userdto.Birthday))
                        user.AdditionalDataOfUser.Birthday = userdto.Birthday;
                    if (!string.IsNullOrWhiteSpace(userdto.Phone))
                        user.AdditionalDataOfUser.Phone = userdto.Phone;
                    if (!string.IsNullOrWhiteSpace(userdto.ShabaNumbers))
                        user.AdditionalDataOfUser.ShabaNumbers = userdto.ShabaNumbers;
                    if (!string.IsNullOrWhiteSpace(userdto.BusinessCode))
                        user.AdditionalDataOfUser.BusinessCode = userdto.BusinessCode;
                    if (!string.IsNullOrWhiteSpace(userdto.RegistrationNumber))
                        user.AdditionalDataOfUser.RegistrationNumber = userdto.RegistrationNumber;

                }
                else if (userdto.Address != null || userdto.Birthday != null || userdto.Gender != null|| userdto.Phone != null || userdto.ShabaNumbers != null || userdto.BusinessCode != null || userdto.RegistrationNumber != null)
                {
                    AdditionalDataOfUser addDataUser = new AdditionalDataOfUser
                    {
                        UserId = user.Id
                    };
                    if (userdto.Gender is not null)
                        addDataUser.Gender = (DataLayer.Entities.Accounts.gender?)userdto.Gender;
                    if (!string.IsNullOrWhiteSpace(userdto.Address))
                        addDataUser.Address = userdto.Address;
                    if (!string.IsNullOrWhiteSpace(userdto.Birthday))
                        addDataUser.Birthday = userdto.Birthday;
                    if (!string.IsNullOrWhiteSpace(userdto.Phone))
                        addDataUser.Phone = userdto.Phone;
                    if (!string.IsNullOrWhiteSpace(userdto.ShabaNumbers))
                        addDataUser.ShabaNumbers = userdto.ShabaNumbers;
                    if (!string.IsNullOrWhiteSpace(userdto.BusinessCode))
                        addDataUser.BusinessCode = userdto.BusinessCode;
                    if (!string.IsNullOrWhiteSpace(userdto.RegistrationNumber))
                        addDataUser.RegistrationNumber = userdto.RegistrationNumber;

                    await additionalDataRepository.AddEntity(addDataUser);
                    await additionalDataRepository.SaveEntity();
                }

                try
                {
                    user.LastUpdate = DateTime.Now;
                    userService.UpDateEntity(user);
                    await userService.SaveEntity();
                    return ResUpdate.Success;

                }
                catch (Exception ex)
                {
                    return ResUpdate.Error;


                }

            }
            return ResUpdate.NotFound;



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
            var user = await userService.GetAllEntity().Include(u=>u.AdditionalDataOfUser).SingleOrDefaultAsync(u=>u.Id==id);
            ReqViewUserDto userdto = new ReqViewUserDto();
            if (user is not null)
            {
                userdto.UserName = user.UserName;
                userdto.FirstName = user.FirstName;
                userdto.LastName = user.LastName;
                userdto.NationalCode = user.NationalCode;
                userdto.Email = user.Email;
                userdto.Mobile = user.Mobile;
                
                userdto.ImageName =PathTools.GetProfilePhotos + user.ImageName;
               
                userdto.AccountType = (accountType?)user.AccountType;
                
                if(user.AdditionalDataOfUser != null)
                {
                    userdto.Gender = (DTOs.UserDtos.gender?)user.AdditionalDataOfUser.Gender;
                    userdto.Address = user.AdditionalDataOfUser.Address;
                    userdto.Phone = user.AdditionalDataOfUser.Phone;
                    userdto.Birthday = user.AdditionalDataOfUser.Birthday;
                    userdto.ShabaNumbers = user.AdditionalDataOfUser.ShabaNumbers;
                    userdto.BusinessCode = user.AdditionalDataOfUser.BusinessCode;
                    userdto.RegistrationNumber = user.AdditionalDataOfUser.RegistrationNumber;
                }
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
            bool flag = false;
            var user= await userService.GetEntity(id);
            if(user != null)
            {
                var isdelete = await userService.DeleteEntity(id); //get id and return true that it means user deleted
                flag = isdelete;
                if (user.ImageName != null)
                    FileUploader.DeleteFile(PathTools.ProfilePhotos + "\\" + user.ImageName);
                var additionalData = await additionalDataRepository.GetAllEntity().SingleOrDefaultAsync(u => u.UserId == id);
                if (additionalData is not null)
                {
                    var isdeleteAdd = await additionalDataRepository.DeleteEntity(additionalData.Id);


                    flag = isdeleteAdd;
                }
                if (flag)
                {
                    try
                    {
                        await userService.SaveEntity();
                        await additionalDataRepository.SaveEntity();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }

                return flag;
            }
            return false;

        }
        #endregion

        #region Add User
        //add user by admin
        //[Authorize(Roles = "نقش مدنظر")]
        public async Task<ResadduserDto> AddUser(ReqAddUserAdminDto userDto)
        {
            if(!string.IsNullOrWhiteSpace(userDto.UserName) && !string.IsNullOrWhiteSpace(userDto.Password))
            {
                if (!await GetUserEmailandUserName(userDto.UserName))
                {
                    User user = new User()
                    {
                        UserName = userDto.UserName.Trim().ToLower(),
                        Password =passwordHelper.EncodePasswordMd5(userDto.Password),
                        IsActive = true,
                        CreateDate=DateTime.Now,
                        LastUpdate=DateTime.Now
                        
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
        public async Task<ResFilterUserDto> GetAlluser(ReqFilterUserDto filter) //for return a list of user that singup in our site for admin
        {
            var usersQuery = userService.GetAllEntity().AsQueryable();
            switch (filter.OrderBy)
            {
                case UsersOrderBy.NameAsc:
                    usersQuery = usersQuery.OrderBy(u => u.FirstName);
                    break;
                case UsersOrderBy.NameDec:
                    usersQuery = usersQuery.OrderByDescending(u => u.FirstName);
                    break;
                case UsersOrderBy.CreateDateAsc:
                    usersQuery = usersQuery.OrderBy(u => u.CreateDate);
                    break;
                case UsersOrderBy.CreateDateDec:
                    usersQuery = usersQuery.OrderByDescending(u => u.CreateDate);
                    break;
                default:
                    break;
            }
            switch (filter.ActiveationFilter)
            {
                case UsersActiveationFilter.Active:
                    usersQuery = usersQuery.Where(u => u.IsActive == true);
                    break;
                case UsersActiveationFilter.NotActive:
                    usersQuery = usersQuery.Where(u => u.IsActive == false);
                    break;
                case UsersActiveationFilter.All:
                    break;
                default:
                    break;
            }
            switch (filter.RemoveFilter)
            {
                case UsersRemoveFilter.Deleted:
                    usersQuery = usersQuery.Where(u => u.IsRemove == true);
                    break;
                case UsersRemoveFilter.NotDeleted:
                    usersQuery = usersQuery.Where(u => u.IsRemove == false);
                    break;
                case UsersRemoveFilter.All:
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(filter.EmailSearchKey))
                usersQuery = usersQuery.Where(u => u.Email.Contains(filter.EmailSearchKey));
            if(!string.IsNullOrWhiteSpace(filter.UserNameSearchKey))
                usersQuery= usersQuery.Where(u => u.UserName.Contains(filter.UserNameSearchKey));

            var count = (int)Math.Ceiling(usersQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list =await usersQuery.Paging(pager).Select(u=> new GetAllUserDto { Email=u.Email,FirstName=u.FirstName,LastName=u.LastName,IsActive=u.IsActive,UserId=u.Id,UserName=u.UserName,IsDelete=u.IsRemove}).ToListAsync();    //use the genric interface options and save values in variable
            
            var result = new ResFilterUserDto();
            result.SetPaging(pager);
            return result.SetUsers(list);
        }

        #endregion

        #region View User Profile
        //profile view for admin
        [Authorize(Roles = "نقش مدنظر")]
        public async Task<ResViewuserAdminDto> ViewUser(long id)
        {
            var user = await userService.GetAllEntity().Include(u => u.AdditionalDataOfUser).SingleOrDefaultAsync(u => u.Id == id);
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
                adminview.ImageName =PathTools.GetProfilePhotos + user.ImageName;
                adminview.AccountType = (accountType?)user.AccountType;
                if (user.AdditionalDataOfUser != null)
                {
                    adminview.Gender = (gender?)user.AdditionalDataOfUser.Gender;
                    adminview.Address = user.AdditionalDataOfUser.Address;
                    adminview.Phone = user.AdditionalDataOfUser.Phone;
                    adminview.Birthday = user.AdditionalDataOfUser.Birthday;
                    adminview.ShabaNumbers = user.AdditionalDataOfUser.ShabaNumbers;
                    adminview.BusinessCode = user.AdditionalDataOfUser.BusinessCode;
                    adminview.RegistrationNumber = user.AdditionalDataOfUser.RegistrationNumber;
                }
            }
            return adminview;
        }


        #endregion

        #endregion


    }
}
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class AdminService : IAdminService
    {
        #region Constructor
        private IGenericReopsitories<Admin> adminRepository;
        //  private IGenericReopsitories<AccounInRole> accounInRoleRepository;
        // private IGenericReopsitories<Role> RoleRepository;
        private IRoleService roleService;
        private IPasswordHelper passwordHelper;
        private IMapper mapper;
        private IMailService mailService;

        public AdminService(IRoleService roleService, IMailService mailService, IMapper mapper, IGenericReopsitories<Admin> adminRepository, IPasswordHelper passwordHelper)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
            this.passwordHelper = passwordHelper;
            this.mailService = mailService;
            this.roleService = roleService;
        }
        #endregion

        #region Admin management
        public async Task<bool> DeleteAdmin(long adminId)
        {
            
            try
            {
                await adminRepository.DeleteEntity(adminId);
                await adminRepository.SaveEntity();
                var res = await roleService.DeleteAdminRole(adminId);
                
                return res;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //public async Task<Role> GetAdminRole(long adminId)
        //{
        //    var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove==false);
        //    return role.Role;
        //}
        

        public async Task<resUpdateAdmin> UpdateAdmin(ReqUpdateAdminDto req)
        {
            var admin = await adminRepository.GetEntity(req.adminId);
            if (admin == null)
                return resUpdateAdmin.NotFound;

            if (!string.IsNullOrWhiteSpace(req.Address))
                admin.Address = req.Address;
            if (!string.IsNullOrWhiteSpace(req.FirstName))
                admin.FirstName = req.FirstName;
            if (!string.IsNullOrWhiteSpace(req.LastName))
                admin.LastName = req.LastName;
            if (!string.IsNullOrWhiteSpace(req.Email))
                admin.Email = req.Email;
            if (!string.IsNullOrWhiteSpace(req.Mobile))
                admin.Mobile = req.Mobile;
            if (!string.IsNullOrWhiteSpace(req.NationalCode))
                admin.NationalCode = req.NationalCode;
            if (!string.IsNullOrWhiteSpace(req.ImageName))
                admin.ImageName = req.ImageName;
            if (!string.IsNullOrWhiteSpace(req.Birthday))
                admin.Birthday = req.Birthday;
            if (req.Gender != null)
                admin.Gender = (DataLayer.Entities.BaseEntities.gender?)req.Gender;
            if (!string.IsNullOrWhiteSpace(req.UserName))
                admin.UserName = req.UserName;

            //update admin role
            if (!string.IsNullOrWhiteSpace(req.RoleName))
            {
                var role = await roleService.GetRoleByName(req.RoleName);
                var accountinrole =await roleService.GetAdminInRole(req.adminId);
                accountinrole.RoleId = role.Id;
                var res=await roleService.UpdateAdminRole(accountinrole);


            }
            try
            {
                admin.LastUpdate = DateTime.Now;
                adminRepository.UpDateEntity(admin);
                await adminRepository.SaveEntity();
                return resUpdateAdmin.Success;
            }
            catch (Exception)
            {

                return resUpdateAdmin.Error;
            }
        }
   
        public async Task<ResViewAdminDto> GetAdminById(long adminId)
        {
            var admin =await adminRepository.GetEntity(adminId);
            var adminRole=await roleService.GetAdminRole(adminId);
            ResViewAdminDto res = new ResViewAdminDto
            {
                Address = admin.Address,
                Birthday = admin.Birthday,
                CreateDate = admin.CreateDate,
                LastName = admin.LastName,
                FirstName = admin.FirstName,
                LastUpdate = admin.LastUpdate,
                Email = admin.Email,
                Gender = admin.Gender.Value.ToString(),
                Id = admin.Id,
                ImageName = admin.ImageName,
                IsRemove = admin.IsRemove,
                Mobile = admin.Mobile,
                NationalCode = admin.NationalCode,      
                UserName = admin.UserName,
                RoleName = adminRole.Title
            };
            
            return res;
        }

        public async Task<RessingupDto> RegisterAdmin(ReqSingupUserDto req)
        {
            var check =await IsAdminExist(req.UserName.Trim().ToLower(), req.Email.ToLower().Trim());
            if (check)
                return RessingupDto.Exist;
            Admin newAdmin= new Admin()
            {
                UserName = req.UserName,
                Email = req.Email,
                FirstName=req.Name,
                LastName=req.Family,
                Mobile=req.phone,
                Password=passwordHelper.EncodePasswordMd5(req.Password),
                CreateDate=DateTime.Now,
                LastUpdate=DateTime.Now
            };

            
            try
            {
               
                await adminRepository.AddEntity(newAdmin);
                await adminRepository.SaveEntity();
                var accountInRole = new AccounInRole
                {
                    AdminId = newAdmin.Id,
                    RoleId = req.AdminRoleId.Value,
                    CreateDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                var res= await roleService.AddRoleToAdmin(accountInRole);
                if(res)
                    return RessingupDto.success;
                return RessingupDto.Failed;

            }
            catch (Exception)
            {

                return RessingupDto.Failed;
            }
           
        }
        public async Task<bool> IsAdminExist(string UserName,string Email)
        {
            return await adminRepository.GetAllEntity().AnyAsync(a=>(a.UserName==UserName||a.Email==Email)&& a.IsRemove==false);
        }

        #region Reset password
        public async Task<bool> ResetPassword(ReqResetPassword request)
        {
            try
            {
                var admin = await adminRepository.GetEntity(request.UserId);
                admin.Password = passwordHelper.EncodePasswordMd5(request.Password);
                admin.LastUpdate = DateTime.Now;
                adminRepository.UpDateEntity(admin);
                await adminRepository.SaveEntity();
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
            var admin = await GetAdminByEmailOrUserName(EmailOrUserName.ToLower().Trim());
            if (admin is not null)
            {
                var res = await mailService.SendResetPasswordEmailAsync(new SendEmailDto { Parameter = admin.Id.ToString(), ToEmail = admin.Email, UserName = admin.UserName });
                if (res)
                    return ResLoginDto.Success;
                else
                    return ResLoginDto.Error;
            }
            return ResLoginDto.IncorrectData;

        }
        #endregion

        #region Get admin by email Or username
        public async Task<Admin> GetAdminByEmailOrUserName(string EmailOrUserName)
        {
            return await adminRepository.GetAllEntity().SingleOrDefaultAsync(u => u.Email == EmailOrUserName.ToLower().Trim() || u.UserName == EmailOrUserName.ToLower().Trim() && u.IsRemove == false);
        }

        #endregion
        #endregion


        #region User management


        #region add
        //add user by admin
        [Authorize(Roles = "نقش مدنظر")]
        public async Task AddUser(ReqSingupUserDto userDto)
        {
            //connect user model options to userdto options model
            User user = new User()
            {
                UserName = userDto.UserName,
                FirstName = userDto.Name,
                LastName = userDto.Family,
                Password = userDto.Password,
                Phone = userDto.phone,
                Email = userDto.Email,
                ActivationCode = new Guid().ToString()
            };

            await userService.AddEntity(user);
            await userService.SaveEntity();
            await mailService.SendActivationCodeEmail(new SendEmailDto { ToEmail = userDto.Email, UserName = userDto.UserName, Parameter = user.ActivationCode });

        }
        #endregion


        #region delete user
        public Task<bool> DeleteUser(long id)
        {
            throw new NotImplementedException();
        }
        #endregion





        #endregion


        #region login
        public async Task<Admin> Login(ReqLoginDto req)
        {
            req.Password = passwordHelper.EncodePasswordMd5(req.Password);
            var admin = await adminRepository.GetAllEntity().SingleOrDefaultAsync(a => (a.UserName == req.UserName || a.Email == req.UserName) && a.Password == req.Password && a.IsRemove == false);
            return admin;

        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            adminRepository.Dispose();
            roleService.Dispose();
        }

   

        #endregion


    }
}

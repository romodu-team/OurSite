using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
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
        private IGenericReopsitories<AccounInRole> accounInRoleRepository;
        private IGenericReopsitories<Role> RoleRepository;
        private IPasswordHelper passwordHelper;
        private IMapper mapper;

        public AdminService(IMapper mapper, IGenericReopsitories<Role> RoleRepository, IGenericReopsitories<Admin> adminRepository, IPasswordHelper passwordHelper, IGenericReopsitories<AccounInRole> accounInRoleRepository)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
            this.passwordHelper = passwordHelper;
            this.accounInRoleRepository = accounInRoleRepository;
            this.RoleRepository = RoleRepository;
        }
        #endregion

        #region Admin management
        public async Task<bool> DeleteAdmin(long adminId)
        {
            var accountInRole = await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove == false);


            try
            {
                if (accountInRole != null)
                {
                    accountInRole.IsRemove = true;
                    accountInRole.LastUpdate = DateTime.Now;
                    await accounInRoleRepository.DeleteEntity(accountInRole.Id);
                    await accounInRoleRepository.SaveEntity();
                }
                await adminRepository.DeleteEntity(adminId);
                await adminRepository.SaveEntity();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Role> GetAdminRole(long adminId)
        {
            var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r => r.AdminId == adminId);
            return role.Role;
        }
        

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
                var role = await RoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.Name == req.RoleName);
                var accountinrole = await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(a => a.AdminId == req.adminId && a.IsRemove == false);
                accountinrole.RoleId = role.Id;
                accountinrole.LastUpdate = DateTime.Now;
                accounInRoleRepository.UpDateEntity(accountinrole);


            }
            try
            {
                admin.LastUpdate = DateTime.Now;
                adminRepository.UpDateEntity(admin);
                await adminRepository.SaveEntity();
                await accounInRoleRepository.SaveEntity();
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
            var adminRole=await GetAdminRole(adminId);
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
                await accounInRoleRepository.AddEntity(accountInRole);
                await accounInRoleRepository.SaveEntity();
                return RessingupDto.success;

            }
            catch (Exception)
            {

                return RessingupDto.Failed;
            }
           
        }
        public async Task<bool> IsAdminExist(string UserName,string Email)
        {
            return await adminRepository.GetAllEntity().AnyAsync(a=>a.UserName==UserName||a.Email==Email);
        }
        #endregion

        #region User management
        public Task AddUser(ReqSingupUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(long id)
        {
            throw new NotImplementedException();
        }
        public Task<ResViewuserAdminDto> ViewUser(string find)
        {
            throw new NotImplementedException();
        }
        public Task GetAlluser()
        {
            throw new NotImplementedException();
        }
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
            accounInRoleRepository.Dispose();
            RoleRepository.Dispose();
        }

   

        #endregion

    }
}

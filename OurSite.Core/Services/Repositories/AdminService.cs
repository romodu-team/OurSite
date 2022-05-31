using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
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

        private IGenericReopsitories<Admin> adminRepository;
        private IGenericReopsitories<AccounInRole> accounInRoleRepository;
        private IGenericReopsitories<Role> RoleRepository;
        private IPasswordHelper passwordHelper;
        private IMapper mapper;
        public AdminService(IGenericReopsitories<Role> RoleRepository, IMapper mapper, IGenericReopsitories<Admin> adminRepository, IPasswordHelper passwordHelper, IGenericReopsitories<AccounInRole> accounInRoleRepository)
        {
            this.adminRepository = adminRepository;
            this.passwordHelper = passwordHelper;
            this.accounInRoleRepository = accounInRoleRepository;
            this.mapper = mapper;
            this.RoleRepository = RoleRepository;
        }
        public Task AddUser(ReqSingupUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAdmin(long adminId)
        {
            var accountInRole =await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove == false);
           
           
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

        public Task<bool> DeleteUser(long id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            adminRepository.Dispose();
            accounInRoleRepository.Dispose();

        }

        public async Task<Role> GetAdminRole(long adminId)
        {
            var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r => r.AdminId == adminId);
            return role.Role;
        }

        public Task GetAlluser()
        {
            throw new NotImplementedException();
        }

        public async Task<Admin> Login(ReqLoginDto req)
        {
            req.Password = passwordHelper.EncodePasswordMd5(req.Password);
            var admin = await adminRepository.GetAllEntity().SingleOrDefaultAsync(a => (a.UserName == req.UserName || a.Email == req.UserName) && a.Password == req.Password && a.IsRemove == false);
            return admin;

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
                var role =await RoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.Name == req.RoleName);
                var accountinrole =await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(a => a.AdminId == req.adminId && a.IsRemove == false);
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

        public Task<ResViewuserAdminDto> ViewUser(string find)
        {
            throw new NotImplementedException();
        }
    }
}

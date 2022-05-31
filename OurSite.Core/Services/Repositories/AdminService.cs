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
        private IPasswordHelper passwordHelper;
        public AdminService(IGenericReopsitories<Admin> adminRepository, IPasswordHelper passwordHelper, IGenericReopsitories<AccounInRole> accounInRoleRepository)
        {
            this.adminRepository = adminRepository;
            this.passwordHelper = passwordHelper;
            this.accounInRoleRepository = accounInRoleRepository;
        }
        public Task AddUser(ReqSingupUserDto userDto)
        {
            throw new NotImplementedException();
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

        //public Task<Admin> GetAdminByUserPass(ReqLoginDto req)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Role> GetAdminRole(long adminId)
        {
            var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r=>r.AdminId==adminId);
            return role.Role;
        }

        public Task GetAlluser()
        {
            throw new NotImplementedException();
        }

        public async Task<Admin> Login(ReqLoginDto req)
        {
            req.Password = passwordHelper.EncodePasswordMd5(req.Password);
            var admin = await adminRepository.GetAllEntity().SingleOrDefaultAsync(a=>(a.UserName==req.UserName || a.Email==req.UserName)&& a.Password==req.Password&& a.IsRemove==false);
            return admin;
               
        }

        public Task<ResViewuserAdminDto> ViewUser(string find)
        {
            throw new NotImplementedException();
        }
    }
}

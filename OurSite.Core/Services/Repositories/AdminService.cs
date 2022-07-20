using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.Utilities.Extentions.Paging;

namespace OurSite.Core.Services.Repositories
{
    public class AdminService : IAdminService
    {
        #region Constructor
        private IGenericReopsitories<Admin> adminRepository;
        private IGenericReopsitories<AdditionalDataOfAdmin> AdditionalDataRepository;
        //  private IGenericReopsitories<AccounInRole> accounInRoleRepository;
        // private IGenericReopsitories<Role> RoleRepository;
        private IRoleService roleService;
        private IPasswordHelper passwordHelper;
        private IMapper mapper;
        private IMailService mailService;

        public AdminService(IGenericReopsitories<AdditionalDataOfAdmin> additionalData, IRoleService roleService, IMailService mailService, IMapper mapper, IGenericReopsitories<Admin> adminRepository, IPasswordHelper passwordHelper)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
            this.passwordHelper = passwordHelper;
            this.mailService = mailService;
            this.roleService = roleService;
            AdditionalDataRepository = additionalData;
        }
        #endregion

        #region Admin management
        #region Delete Admin
        public async Task<bool> DeleteAdmin(long adminId)             //its return true/false -We do not have a real deletion
        {
            bool flag = false;
            var isdelete = await adminRepository.DeleteEntity(adminId);
            await adminRepository.SaveEntity();
            var additionalData = AdditionalDataRepository.GetAllEntity().SingleOrDefaultAsync(u => u.AdminId == adminId).Result;
            flag = isdelete;
            if(additionalData is not null)
            {
                var isdeleteAdd = await AdditionalDataRepository.DeleteEntity(additionalData.Id);
                await AdditionalDataRepository.SaveEntity();

                flag = isdeleteAdd;
            }
            if (flag)
            {
                try
                {

                   
                    var myres = await roleService.DeleteAdminRole(adminId);
                    switch (myres)
                    {
                        case ResDeleteAdminRole.Success:
                            return true;
                        case ResDeleteAdminRole.NotExist:
                            return true;
                        case ResDeleteAdminRole.Faild:
                            return false;
                        default:
                            return false;
                    }
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }
        #endregion

        #region change admin status
        public async Task<bool> ChangeAdminStatus(long adminId)
        {
            try
            {
                var admin = await adminRepository.GetEntity(adminId);
                admin.IsActive = !admin.IsActive;
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

        #region Update admin profile by self/admin
        public async Task<ResUpdate> UpdateAdmin(ReqUpdateAdminDto req, long id)
        {
         //   var admin = await adminRepository.GetEntity(id);
            var admin =await adminRepository.GetAllEntity().Where(a => a.Id == id).Include(a => a.additionalDataOfAdmin).SingleOrDefaultAsync();
            if (admin == null)
                return ResUpdate.NotFound;

            if (!string.IsNullOrWhiteSpace(req.FirstName))
                admin.FirstName = req.FirstName;
            if (!string.IsNullOrWhiteSpace(req.LastName))
                admin.LastName = req.LastName;
            if (!string.IsNullOrWhiteSpace(req.Email))
                admin.Email = req.Email.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(req.Mobile))
                admin.Mobile = req.Mobile.Trim();
            if (!string.IsNullOrWhiteSpace(req.NationalCode))
                admin.NationalCode = req.NationalCode.Trim();
            if (!string.IsNullOrWhiteSpace(req.UserName))
                admin.UserName = req.UserName.ToLower().Trim();

            if(admin.additionalDataOfAdmin is not null)
            {
                if (!string.IsNullOrWhiteSpace(req.Address))
                    admin.additionalDataOfAdmin.Address = req.Address;
                if (!string.IsNullOrWhiteSpace(req.Birthday))
                    admin.additionalDataOfAdmin.Birthday = req.Birthday;
                if (req.Gender != null)
                    admin.additionalDataOfAdmin.Gender = (DataLayer.Entities.Accounts.gender?)req.Gender;
            }
            else if (req.Address != null || req.Birthday != null || req.Gender != null)
            {
                AdditionalDataOfAdmin addDataAdmin = new AdditionalDataOfAdmin
                {
                    AdminId = admin.Id
                };
                if (!string.IsNullOrWhiteSpace(req.Address))
                    addDataAdmin.Address = req.Address;
                if (!string.IsNullOrWhiteSpace(req.Birthday))
                    addDataAdmin.Birthday = req.Birthday;
                if (req.Gender != null)
                    addDataAdmin.Gender = (DataLayer.Entities.Accounts.gender?)req.Gender;
                await AdditionalDataRepository.AddEntity(addDataAdmin);
                await AdditionalDataRepository.SaveEntity();
            }
            


            //update admin role
            if (!string.IsNullOrWhiteSpace(req.Roleid))
            {
                var accountinrole = await roleService.GetAdminInRole(id);
                accountinrole.RoleId = Convert.ToInt64(req.Roleid);
                var res = await roleService.UpdateAdminRole(accountinrole);


            }
            try
            {
                admin.LastUpdate = DateTime.Now;
                adminRepository.UpDateEntity(admin);
                await adminRepository.SaveEntity();
                return ResUpdate.Success;
            }
            catch (Exception)
            {

                return ResUpdate.Error;
            }


        }
        public async Task<resFileUploader> ProfilePhotoUpload(IFormFile photo, long UserId)
        {
            var result = await FileUploader.UploadFile(PathTools.ProfilePhotos, photo, 3);
            if (result.Status == resFileUploader.Success)
            {
                Admin admin = await adminRepository.GetEntity(UserId);
                admin.ImageName = result.FileName;
                adminRepository.UpDateEntity(admin);
                await adminRepository.SaveEntity();
            }
            return result.Status;
        }
        #endregion

        #region Admin finder with id 
        public async Task<ResViewAdminDto> GetAdminById(long adminId)
        {
            var admin = await adminRepository.GetEntity(adminId);
            var adminRole = await roleService.GetAdminRole(adminId);
            var additionalData = AdditionalDataRepository.GetAllEntity().SingleOrDefault(a => a.AdminId == admin.Id);
            ResViewAdminDto res = new ResViewAdminDto
            {
                
                CreateDate = admin.CreateDate,
                LastName = admin.LastName,
                FirstName = admin.FirstName,
                LastUpdate = admin.LastUpdate,
                Email = admin.Email,
                Id = admin.Id,
                ImageName =PathTools.GetProfilePhotos + admin.ImageName,
                IsRemove = admin.IsRemove,
                Mobile = admin.Mobile,
                NationalCode = admin.NationalCode,
                UserName = admin.UserName,
                RoleName = adminRole.Title
            };
            if (additionalData != null)
            {
                res.Address = additionalData.Address;
                res.Birthday = additionalData.Birthday;
                if(additionalData.Gender is not null)
                    res.Gender = additionalData.Gender.Value.ToString();
            }
            return res;
        }

        #endregion

        #region Add new admin
        public async Task<RessingupDto> RegisterAdmin(ReqRegisterAdminDto req)
        {
            var check = await IsAdminExist(req.UserName.Trim().ToLower(), req.Email.ToLower().Trim());
            if (check)
                return RessingupDto.Exist;
            Admin newAdmin = new Admin()
            {
                UserName = req.UserName.Trim().ToLower(),
                Email = req.Email.Trim().ToLower(),
                FirstName=req.Name,
                LastName=req.Family,
                Mobile=req.Mobile.Trim(),
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
                var res = await roleService.AddRoleToAdmin(accountInRole);
                if (res)
                    return RessingupDto.success;
                return RessingupDto.Failed;

            }
            catch (Exception)
            {

                return RessingupDto.Failed;
            }

        }
        public async Task<bool> IsAdminExist(string UserName, string Email)
        {
            return await adminRepository.GetAllEntity().AnyAsync(a=>(a.UserName==UserName.Trim().ToLower()||a.Email==Email.Trim().ToLower())&& a.IsRemove==false);
        }
        #endregion

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
                var res = await mailService.SendResetPasswordEmailAsync(new SendEmailDto { Parameter = admin.Id.ToString(), ToEmail = admin.Email.Trim(), UserName = admin.UserName });
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

        #region Admin list
        public async Task<ResFilterAdminDto> GetAllAdmin(ReqFilterUserDto filter)
        {
            var adminQuery = adminRepository.GetAllEntity().AsQueryable();
            switch (filter.OrderBy)
            {
                case UsersOrderBy.NameAsc:
                    adminQuery = adminQuery.OrderBy(u => u.FirstName);
                    break;
                case UsersOrderBy.NameDec:
                    adminQuery = adminQuery.OrderByDescending(u => u.FirstName);
                    break;
                case UsersOrderBy.CreateDateAsc:
                    adminQuery = adminQuery.OrderBy(u => u.CreateDate);
                    break;
                case UsersOrderBy.CreateDateDec:
                    adminQuery = adminQuery.OrderByDescending(u => u.CreateDate);
                    break;
                default:
                    break;
            }
            switch (filter.ActiveationFilter)
            {
                case UsersActiveationFilter.Active:
                    adminQuery = adminQuery.Where(u => u.IsActive == true);
                    break;
                case UsersActiveationFilter.NotActive:
                    adminQuery = adminQuery.Where(u => u.IsActive == false);
                    break;
                case UsersActiveationFilter.All:
                    break;
                default:
                    break;
            }
            switch (filter.RemoveFilter)
            {
                case UsersRemoveFilter.Deleted:
                    adminQuery = adminQuery.Where(u => u.IsRemove == true);
                    break;
                case UsersRemoveFilter.NotDeleted:
                    adminQuery = adminQuery.Where(u => u.IsRemove == false);
                    break;
                case UsersRemoveFilter.All:
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(filter.EmailSearchKey))
                adminQuery = adminQuery.Where(u => u.Email.Contains(filter.EmailSearchKey));
            if (!string.IsNullOrWhiteSpace(filter.UserNameSearchKey))
                adminQuery = adminQuery.Where(u => u.UserName.Contains(filter.UserNameSearchKey));

            var count = (int)Math.Ceiling(adminQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await adminQuery.Paging(pager).Select(x => new GetAllAdminDto { Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, AdminId = x.Id, UserName = x.UserName, IsDelete = x.IsRemove }).ToListAsync();    //use the genric interface options and save values in variable

            var result = new ResFilterAdminDto();
            result.SetPaging(pager);
            return result.SetAdmins(list);

      
        }

        #endregion

        #region Update Admin Role
        public async Task<ResUpdate> UpdateAdminRole(long adminId, long RoleId)
        {
            var Admin =await adminRepository.GetEntity(adminId);
            if (Admin is null)
                return ResUpdate.NotFound;
            var Role = await roleService.GetRoleById(RoleId);
            if (Role is null)
                return ResUpdate.RoleNotFound;

            var adminRole =await roleService.GetAdminInRole(adminId);
            if(adminRole is not null)
            {
                adminRole.RoleId = RoleId;
                var res=await roleService.UpdateAdminRole(adminRole);
                if (res)
                    return ResUpdate.Success;
                return ResUpdate.Error;
            }
            else
            {
                var adminInRole = new AccounInRole() { AdminId=adminId,RoleId=RoleId};
                var res =await roleService.AddRoleToAdmin(adminInRole);
                if (res)
                    return ResUpdate.Success;
                return ResUpdate.Error;
            }
        }

        #endregion
        #endregion



        #region login
        public async Task<Admin> Login(ReqLoginDto req)
        {
            req.Password = passwordHelper.EncodePasswordMd5(req.Password);
            req.UserName = req.UserName.Trim().ToLower();
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

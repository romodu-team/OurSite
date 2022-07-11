using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;


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
        #region Delete Admin
        public async Task<bool> DeleteAdmin(long adminId)             //its return true/false -We do not have a real deletion
        {
            var isdelete = await adminRepository.DeleteEntity(adminId);

            if (isdelete)
            {
                try
                {

                    await adminRepository.SaveEntity();
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


        #region Update admin profile by self/admin
        public async Task<ResUpdate> UpdateAdmin(ReqUpdateAdminDto req, long id)
        {
            var admin = await adminRepository.GetEntity(id);
            if (admin == null)
                return ResUpdate.NotFound;

            if (!string.IsNullOrWhiteSpace(req.Address))
                admin.Address = req.Address;
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
            if (!string.IsNullOrWhiteSpace(req.ImageName))
                admin.ImageName = req.ImageName;
            if (!string.IsNullOrWhiteSpace(req.Birthday))
                admin.Birthday = req.Birthday;
            if (req.Gender != null)
                admin.Gender = (DataLayer.Entities.BaseEntities.gender?)req.Gender;
            if (!string.IsNullOrWhiteSpace(req.UserName))
                admin.UserName = req.UserName.ToLower().Trim();

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

        #endregion

        #region Admin founder with id 
        public async Task<ResViewAdminDto> GetAdminById(long adminId)
        {
            var admin = await adminRepository.GetEntity(adminId);
            var adminRole = await roleService.GetAdminRole(adminId);
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

        #endregion

        #region Add new admin
        public async Task<RessingupDto> RegisterAdmin(ReqSingupUserDto req)
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
        public async Task<List<GetAllAdminDto>> GetAllAdmin()
        {
            var list = await adminRepository.GetAllEntity().Where(x => x.IsRemove == false).Select(x => new GetAllAdminDto { Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, AdminId = x.Id, UserName = x.UserName, IsDelete = x.IsRemove }).ToListAsync();
            return list;
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

using System;
using Microsoft.EntityFrameworkCore;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.Comments;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.ContactWithUs;
using OurSite.DataLayer.Entities.ImageGalleries;
using OurSite.DataLayer.Entities.NotificationModels;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Entities.RatingModel;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Entities.WorkSamples;
using OurSite.OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.DataLayer.Contexts
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdditionalDataOfAdmin> AdditionalDataOfAdmin { get; set; }
        public DbSet<AdditionalDataOfUser> AdditionalDataOfUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccounInRole> AccounInRoles { get; set; }
        public DbSet<ContactWithUs> contactWithUs { get; set; }
        public DbSet<ConsultationRequest> consultationRequest { get; set; }
        public DbSet<ItemSelected> itemsSelecteds { get; set; }
        public DbSet<CheckBoxs> checkBoxes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SelectedProjectPlan> selectedProjectPlans { get; set; }
        public DbSet<WorkSample> WorkSamples { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ImageGallery> ImageGallery { get; set; }
        public DbSet<ProjectFeatures> ProjectFeatures { get; set; }
        public DbSet<WorkSampleCategory> WorkSampleCategories { get; set; }
        public DbSet<WorkSampleInCategory> WorkSampleInCategories { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketDiscussion> TicketDiscussions { get; set; }
        public DbSet<TicketModel> Ticket { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
       .SelectMany(t => t.GetForeignKeys())
       .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            
            base.OnModelCreating(modelBuilder);
            //دسته بندی (پدر) هاشون مونده
            #region permission data seed
            modelBuilder.Entity<Permission>().HasData(new Permission {CreateDate=DateTime.Now,LastUpdate=DateTime.Now, Id=1,PermissionTitle = "افزودن ادمین جدید", PermissionName = "Permission.RegisterAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id =2, PermissionTitle = "فعال/غیرفعال کردن ادمین ها", PermissionName = "Permission.ChangeAdminStatus", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 3, PermissionTitle = "حذف ادمین", PermissionName = "Permission.DeleteAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 4, PermissionTitle = "مشاهده حساب ادمین دیگر", PermissionName = "Permission.ViewAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 5, PermissionTitle = "مشاهده لیست تمام ادمین ها", PermissionName = "Permission.ViewAllAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 6, PermissionTitle = "ویرایش ادمین دیگر", PermissionName = "Permission.UpdateAnotherAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 7, PermissionTitle = "مشاهده حساب یک کاربر", PermissionName = "Permission.ViewUser", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 8, PermissionTitle = "مشاهده لیست تمام کاربران", PermissionName = "Permission.ViewAllUser", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 9, PermissionTitle = "فعال/غیرفعال کردن کاربران", PermissionName = "Permission.ChangeUserStatus", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 10, PermissionTitle = "افزودن کاربر جدید", PermissionName = "Permission.RegisterUserByAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 11, PermissionTitle = "ویرایش حساب کاربر", PermissionName = "Permission.UpdateUserByAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 12, PermissionTitle = "حذف حساب کاربر", PermissionName = "Permission.DeleteUserByAdmin", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 13, PermissionTitle = "افزودن نقش جدید", PermissionName = "Permission.AddNewRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 14, PermissionTitle = "حذف نقش", PermissionName = "Permission.DeleteRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 15, PermissionTitle = "ویرایش نقش", PermissionName = "Permission.UpdateRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 16, PermissionTitle = "مشاهده یک نقش", PermissionName = "Permission.ViewRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 17, PermissionTitle = "مشاهده لیست تمام نقش ها", PermissionName = "Permission.ViewAllRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 18, PermissionTitle = "ویرایش نقش یک ادمین", PermissionName = "Permission.UpdateAdminRole", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 19, PermissionTitle = "مشاهده لیست تمام سطح دسترسی ها", PermissionName = "Permission.ViewAllPermissions", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 20, PermissionTitle = "ویرایش سطح دسترسی یک نقش", PermissionName = "Permission.UpdateRolePermissions", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 21, PermissionTitle = "مشاهده لیست فرم های درخواست مشاوره", PermissionName = "Permission.ViewAllConsultationFroms", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 22, PermissionTitle = "مشاهده فرم درخواست مشاوره", PermissionName = "Permission.ViewConsultationFrom", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 23, PermissionTitle = "مشاهده لیست فرم های تماس با ما", PermissionName = "Permission.ViewAllContactWithUs", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 24, PermissionTitle = "ارسال پاسخ به فرم تماس با ما", PermissionName = "Permission.AnswerToContactWithUs", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 25, PermissionTitle = "ایجاد پروژه جدید", PermissionName = "Permission.AdminCreateProject", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 26, PermissionTitle = "ویرایش پروژه", PermissionName = "Permission.AdminEditProject", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 27, PermissionTitle = "حذف پروژه", PermissionName = "Permission.AdminDeleteProject", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 28, PermissionTitle = "مشاهده یک پروژه", PermissionName = "Permission.AdminViewProject", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 29, PermissionTitle = "اپلود قرارداد برای پروژه", PermissionName = "Permission.AdminUploadProjectContractst", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 30, PermissionTitle = "مشاهده لیست پروژه ها", PermissionName = "Permission.AdminViewAllProject", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 31, PermissionTitle = "ایجاد تیکت جدید", PermissionName = "Permission.CreateTicket", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 32, PermissionTitle = "ویرایش تیکت", PermissionName = "Permission.UpdateTicket", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 33, PermissionTitle = "حذف تیکت", PermissionName = "Permission.DeleteTicket", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 34, PermissionTitle = "تغییر وضعیت یک تیکت", PermissionName = "Permission.ChangeTicketStatus", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 35, PermissionTitle = "مشاهده لیست تمام تیکت ها", PermissionName = "Permission.ViewAllTickets", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 36, PermissionTitle = "حذف پیام از یک تیکت", PermissionName = "Permission.DeleteDiscussion", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 37, PermissionTitle = "مدیریت دسته بندی تیکت ها", PermissionName = "Permission.ManageTicketCategory", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 38, PermissionTitle = "مدیریت اولویت تیکت ها", PermissionName = "Permission.ManageTicketPriority", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 39, PermissionTitle = "مدیریت وضعیت تیکت ها", PermissionName = "Permission.ManageTicketStatus", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 40, PermissionTitle = "مدیریت کادر های انتخابی", PermissionName = "Permission.ManageCheckBox", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 41, PermissionTitle = "افزودن تصویر به گالری تصاویر", PermissionName = "Permission.AddImageToGallery", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 42, PermissionTitle = "حذف تصویر از گالری تصاویر", PermissionName = "Permission.DeleteImageFromGallery", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 43, PermissionTitle = "ایجاد اعلان جدید", PermissionName = "Permission.CreateNotification", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 44, PermissionTitle = "ویرایش یک اعلان", PermissionName = "Permission.UpdateNotification", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 45, PermissionTitle = "ایجاد فاکتور جدید", PermissionName = "Permission.CreatePayment", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 46, PermissionTitle = "ویرایش یک فاکتور", PermissionName = "Permission.EditPayment", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 47, PermissionTitle = "حذف یک فاکتور", PermissionName = "Permission.DeletePayment", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 48, PermissionTitle = "ایجاد نمونه کار جدید", PermissionName = "Permission.CreateWorkSample", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 49, PermissionTitle = "ویرایش یک نمونه کار", PermissionName = "Permission.EditWorkSample", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id =50, PermissionTitle = "حذف یک نمونه کار", PermissionName = "Permission.DeleteWorkSample", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 51, PermissionTitle = "ایجاد دسته بندی نمونه کار جدید", PermissionName = "Permission.CreateWorkSampleCategory", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 52, PermissionTitle = "حذف دسته بندی نمونه کار", PermissionName = "Permission.DeleteWorkSampleCategory", UUID = Guid.NewGuid() });
            modelBuilder.Entity<Permission>().HasData(new Permission { CreateDate = DateTime.Now, LastUpdate = DateTime.Now, Id = 53, PermissionTitle = "ویرایش دسته بندی نمونه کار", PermissionName = "Permission.UpdateWorkSampleCategory", UUID = Guid.NewGuid() });
            #endregion


        }
    }
}


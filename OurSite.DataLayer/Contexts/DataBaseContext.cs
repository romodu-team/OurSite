using System;
using Microsoft.EntityFrameworkCore;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.ContactWithUs;
using OurSite.DataLayer.Entities.Departments;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Entities.TicketMessageing;

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
        public DbSet<Department> departments { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<TicketMessage> ticketMessages { get; set; }
        public DbSet<ItemSelected> itemsSelecteds { get; set; }
        public DbSet<CheckBoxs> checkBoxes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
       .SelectMany(t => t.GetForeignKeys())
       .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Department>()
              .HasData(
               new Department { Id = 1, DepartmentName = "Financial", DepartmentTitle = "بخش مالی", CreateDate = DateTime.Now, LastUpdate = DateTime.Now },
               new Department { Id = 2, DepartmentName = "Technical support", DepartmentTitle = "پشتیبانی فنی", CreateDate = DateTime.Now, LastUpdate = DateTime.Now },
               new Department { Id = 3, DepartmentName = "Other", DepartmentTitle = "سایر", CreateDate = DateTime.Now, LastUpdate = DateTime.Now }
               );
        }
    }
}


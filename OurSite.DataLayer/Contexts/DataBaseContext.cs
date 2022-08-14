using System;
using Microsoft.EntityFrameworkCore;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.Comments;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.ContactWithUs;
using OurSite.DataLayer.Entities.ImageGalleries;
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
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
       .SelectMany(t => t.GetForeignKeys())
       .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            
            base.OnModelCreating(modelBuilder);

        }
    }
}


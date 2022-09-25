using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Utilities
{
    public class StaticPermissions
    {
        private readonly IGenericRepository<Permission> _generic;
        public StaticPermissions(IGenericRepository<Permission> generic)
        {
            _generic = generic;
        }

        #region Admin Management
        public const string PermissionRegisterAdmin = "Permission.RegisterAdmin";
        public const string PermissionChangeAdminStatus = "Permission.ChangeAdminStatus";
        public const string PermissionDeleteAdmin = "Permission.DeleteAdmin";
        public const string PermissionViewAdmin = "Permission.ViewAdmin";
        public const string PermissionViewAllAdmin = "Permission.ViewAllAdmin";
        public const string PermissionUpdateAnotherAdmin = "Permission.UpdateAnotherAdmin";
        #endregion
        #region Role Management
        public const string PermissionAddNewRole = "Permission.AddNewRole";
        public const string PermissionDeleteRole = "Permission.DeleteRole";
        public const string PermissionUpdateRole = "Permission.UpdateRole";
        public const string PermissionViewRole = "Permission.ViewRole";
        public const string PermissionViewAllRole = "Permission.ViewAllRole";
        public const string PermissionUpdateAdminRole = "Permission.UpdateAdminRole";
        public const string PermissionViewAllPermissions = "Permission.ViewAllPermissions";
        public const string PermissionUpdateRolePermissions = "Permission.UpdateRolePermissions";
        #endregion
        #region User management
        public const string PermissionViewUser = "Permission.ViewUser";
        public const string PermissionViewAllUser = "Permission.ViewAllUser";
        public const string PermissionChangeUserStatus = "Permission.ChangeUserStatus";
        public const string PermissionRegisterUserByAdmin = "Permission.RegisterUserByAdmin";
        public const string PermissionUpdateUserByAdmin = "Permission.UpdateUserByAdmin";
        public const string PermissionDeleteUserByAdmin = "Permission.DeleteUserByAdmin";
        #endregion

        #region Consultation form
        public const string PermissionViewAllConsultationFroms = "Permission.ViewAllConsultationFroms";
        public const string PermissionViewConsultationFrom = "Permission.ViewConsultationFrom";
        #endregion

        #region contact with us
        public const string PermissionViewAllContactWithUs = "Permission.ViewAllContactWithUs";
        public const string PermissionAnswerToContactWithUs = "Permission.AnswerToContactWithUs";

        #endregion
        #region Project
        public const string PermissionAdminCreateProject = "Permission.AdminCreateProject";
        public const string PermissionAdminEditProject = "Permission.AdminEditProject";
        public const string PermissionAdminDeleteProject = "Permission.AdminDeleteProject";
        public const string PermissionAdminViewProject = "Permission.AdminViewProject";
        public const string PermissionAdminUploadProjectContracts = "Permission.AdminUploadProjectContractst";
        public const string PermissionAdminViewAllProject = "Permission.AdminViewAllProject";

        #endregion
        #region Ticket
        public const string PermissionCreateTicket = "Permission.CreateTicket";
        public const string PermissionUpdateTicket = "Permission.UpdateTicket";
        public const string PermissionDeleteTicket = "Permission.DeleteTicket";
        public const string PermissionChangeTicketStatus = "Permission.ChangeTicketStatus";
        public const string PermissionViewAllTickets = "Permission.ViewAllTickets";
        public const string PermissionDeleteDiscussion = "Permission.DeleteDiscussion";
        public const string PermissionManageTicketCategory = "Permission.ManageTicketCategory";
        public const string PermissionManageTicketPriority = "Permission.ManageTicketPriority";
        public const string PermissionManageTicketStatus = "Permission.ManageTicketStatus";

        #endregion
        #region Manage Checkbox
        public const string PermissionManageCheckBox = "Permission.ManageCheckBox";

        #endregion
        #region Image Gallery
        public const string PermissionAddImageToGallery = "Permission.AddImageToGallery";
        public const string PermissionDeleteImageFromGallery = "Permission.DeleteImageFromGallery ";

        #endregion
        #region Notification
        public const string PermissionCreateNotification = "Permission.CreateNotification";
        public const string PermissionUpdateNotification = "Permission.UpdateNotification";

        #endregion
        #region Payment
        public const string PermissionCreatePayment = "Permission.CreatePayment";
        public const string PermissionEditPayment = "Permission.EditPayment";
        public const string PermissionDeletePayment = "Permission.DeletePayment";

        #endregion
        #region Worksample
        public const string PermissionCreateWorkSample = "Permission.CreateWorkSample";
        public const string PermissionEditWorkSample = "Permission.EditWorkSample";
        public const string PermissionDeleteWorkSample = "Permission.DeleteWorkSample";
        public const string PermissionCreateWorkSampleCategory = "Permission.CreateWorkSampleCategory";
        public const string PermissionDeleteWorkSampleCategory = "Permission.DeleteWorkSampleCategory";
        public const string PermissionUpdateWorkSampleCategory = "Permission.UpdateWorkSampleCategory";

        #endregion
        public static List<string> GetPermissions()
        {
            List<string> Permissions = new List<string>();
            Permissions.Add(PermissionRegisterAdmin);
            Permissions.Add(PermissionChangeAdminStatus);
            Permissions.Add(PermissionDeleteAdmin);
            Permissions.Add(PermissionViewAdmin);
            Permissions.Add(PermissionViewAllAdmin);
            Permissions.Add(PermissionUpdateAnotherAdmin);

            Permissions.Add(PermissionViewUser);
            Permissions.Add(PermissionViewAllUser);
            Permissions.Add(PermissionChangeUserStatus);
            Permissions.Add(PermissionRegisterUserByAdmin);
            Permissions.Add(PermissionUpdateUserByAdmin);
            Permissions.Add(PermissionDeleteUserByAdmin);

            Permissions.Add(PermissionAddNewRole);
            Permissions.Add(PermissionDeleteRole);
            Permissions.Add(PermissionUpdateRole);
            Permissions.Add(PermissionViewRole);
            Permissions.Add(PermissionViewAllRole);
            Permissions.Add(PermissionUpdateAdminRole);
            Permissions.Add(PermissionViewAllPermissions);
            Permissions.Add(PermissionUpdateRolePermissions);

            Permissions.Add(PermissionViewAllConsultationFroms);
            Permissions.Add(PermissionViewConsultationFrom);

            Permissions.Add(PermissionViewAllContactWithUs);
            Permissions.Add(PermissionAnswerToContactWithUs);

            Permissions.Add(PermissionAdminCreateProject);
            Permissions.Add(PermissionAdminEditProject);
            Permissions.Add(PermissionAdminDeleteProject);
            Permissions.Add(PermissionAdminViewProject);
            Permissions.Add(PermissionAdminUploadProjectContracts);
            Permissions.Add(PermissionAdminViewAllProject);


            Permissions.Add(PermissionCreateTicket);
            Permissions.Add(PermissionUpdateTicket);
            Permissions.Add(PermissionDeleteTicket);
            Permissions.Add(PermissionChangeTicketStatus);
            Permissions.Add(PermissionViewAllTickets);
            Permissions.Add(PermissionDeleteDiscussion);
            Permissions.Add(PermissionManageTicketCategory);
            Permissions.Add(PermissionManageTicketPriority);
            Permissions.Add(PermissionManageTicketStatus);

            Permissions.Add(PermissionManageCheckBox);
            Permissions.Add(PermissionAddImageToGallery);
            Permissions.Add(PermissionDeleteImageFromGallery);

            Permissions.Add(PermissionCreateNotification);
            Permissions.Add(PermissionUpdateNotification);

            Permissions.Add(PermissionCreatePayment);
            Permissions.Add(PermissionEditPayment);
            Permissions.Add(PermissionDeletePayment);

            Permissions.Add(PermissionCreateWorkSample);
            Permissions.Add(PermissionEditWorkSample);
            Permissions.Add(PermissionDeleteWorkSample);
            Permissions.Add(PermissionCreateWorkSampleCategory);
            Permissions.Add(PermissionDeleteWorkSampleCategory);
            Permissions.Add(PermissionUpdateWorkSampleCategory);

            return Permissions;
        }
    }
}

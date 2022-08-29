using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class Role : BaseEntity
    {
        #region Properties
        [DisplayName("عنوان نقش")]
        [MaxLength(50, ErrorMessage = " عنوان نقش نمی‌تواند بیش از {0} کاراکتر باشد")]
        public string Title { get; set; }

        [Required(ErrorMessage ="این فیلد اجباری است")]
        [DisplayName("نام سیستمی")]
        [MaxLength(50, ErrorMessage = " نام سیستمی نمی‌تواند بیش از {0} کاراکتر باشد")]
        public string Name { get; set; }
        #endregion

        #region Realations
        public ICollection<AccounInRole> AccounInRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion

    }
    public enum ResRole
    {
        Success,
        Faild,
        Error,
        NotFound,
        InvalidInput
    }


}


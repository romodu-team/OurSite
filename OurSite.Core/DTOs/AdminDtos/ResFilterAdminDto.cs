using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ResFilterAdminDto : BasePaging
    {
        public List<GetAllAdminDto>? Admins { get; set; }
        public ResFilterAdminDto SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            PageCount = paging.PageCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            ActivePage = paging.ActivePage;
            return this;
        }
        public ResFilterAdminDto SetAdmins(List<GetAllAdminDto> admins)
        {
            Admins = admins;
            return this;
        }
    }
}

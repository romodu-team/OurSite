using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.UserDtos
{
    public class ResFilterUserDto : BasePaging
    {
        public List<GetAllUserDto>? Users { get; set; }

        public ResFilterUserDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.ActivePage = paging.ActivePage;
            return this;
        }
        public ResFilterUserDto SetUsers(List<GetAllUserDto> users)
        {
            this.Users = users;
            return this;
        }
    }
}

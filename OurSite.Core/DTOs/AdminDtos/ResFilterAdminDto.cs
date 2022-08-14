﻿using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.UserDtos
{
    public class ResFilterAdminDto:BasePaging
    {
        public List<GetAllAdminDto>? Admins { get; set; }
        public ResFilterAdminDto SetPaging(BasePaging paging)
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
        public ResFilterAdminDto SetAdmins(List<GetAllAdminDto> admins)
        {
            this.Admins = admins;
            return this;
        }
    }
}

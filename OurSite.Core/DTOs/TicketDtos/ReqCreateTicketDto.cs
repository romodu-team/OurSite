﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ReqCreateTicketDto
    {
        public string Title { get; set; }
        [MinLength(1)]
        public string Description { get; set; }
        public long TicketCategoryId { get; set; }
        public long TicketStatusId { get; set; }
        public long? TicketPriorityId { get; set; }
        public long? SupporterId { get; set; }
        public long UserId { get; set; }
        public long SenderId { get; set; }
        public long? ProjectId { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}

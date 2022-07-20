using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.UserDtos
{
    public class ReqFilterUserDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? EmailSearchKey { get; set; }
        public string? UserNameSearchKey { get; set; }
        public UsersOrderBy? OrderBy { get; set; }
        public UsersRemoveFilter? RemoveFilter { get; set; }
        public UsersActiveationFilter? ActiveationFilter { get; set; }
        
    }
    public enum UsersOrderBy
    {
        NameAsc,
        NameDec,
        CreateDateAsc,
        CreateDateDec
    }
    public enum UsersActiveationFilter
    {
        Active,
        NotActive,
        All
    }
    public enum UsersRemoveFilter
    {
        Deleted,
        NotDeleted,
        All
    }
}

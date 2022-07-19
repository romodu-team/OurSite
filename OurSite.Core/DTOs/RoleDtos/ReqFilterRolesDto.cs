using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.RoleDtos
{
    public class ReqFilterRolesDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? RoleTitleSearchKey { get; set; }
        public string? RoleNameSearchKey { get; set; }
        public RolesOrderBy? RolesOrderBy { get; set; }
        public RolesRemoveFilter? RolesRemoveFilter { get; set; }

    }
    public enum RolesOrderBy
    {
        NameAsc,
        NameDec,
        CreateDateAsc,
        CreateDateDec
    }

    public enum RolesRemoveFilter
    {
        Deleted,
        NotDeleted,
        All
    }
}

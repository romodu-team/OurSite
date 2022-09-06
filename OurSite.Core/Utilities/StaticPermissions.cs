using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Utilities
{
    public class StaticPermissions
    {
      public const string PermissionEditPayment = "Permission.EditPayment";
        public static List<string> GetPermissions()
        {
            List<string> strings = new List<string>();
            strings.Add(PermissionEditPayment);
            return strings;
        }
    }
}

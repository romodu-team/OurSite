using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OurSite.Core.Utilities
{
    public static class JsonStatusResponse
    {
        public static JsonResult Success()
        {
            return new JsonResult(new { Status = "Success" });
        }

        public static JsonResult Success(object ReturnData)
        {
            return new JsonResult(new { Status = "Success", Data = ReturnData });
        }

        public static JsonResult Error()
        {
            return new JsonResult(new { Status = "Error" });
        }

        public static JsonResult Error(object ReturnData)
        {
            return new JsonResult(new { Status = "Error", Data = ReturnData });
        }
        public static JsonResult NotFound()
        {
            return new JsonResult(new { Status = "NotFound" });
        }

        public static JsonResult NotFound(object ReturnData)
        {
            return new JsonResult(new { Status = "NotFound", Data = ReturnData });
        }
    }
}

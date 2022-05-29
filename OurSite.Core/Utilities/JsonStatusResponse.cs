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
        public static JsonResult Success(string message)
        {
            return new JsonResult(new { Status = "Success",Message=message });
        }

        public static JsonResult Success(object ReturnData, string message)
        {
            return new JsonResult(new { Status = "Success", Data = ReturnData, Message = message });
        }

        public static JsonResult Error(string message)
        {
            return new JsonResult(new { Status = "Error", Message = message });
        }

        public static JsonResult Error(object ReturnData, string message)
        {
            return new JsonResult(new { Status = "Error", Data = ReturnData, Message = message });
        }
        public static JsonResult NotFound(string message)
        {
            return new JsonResult(new { Status = "NotFound", Message = message });
        }

        public static JsonResult NotFound(object ReturnData, string message)
        {
            return new JsonResult(new { Status = "NotFound", Data = ReturnData, Message = message });
        }

    }
}

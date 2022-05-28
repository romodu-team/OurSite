using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IGenericReopsitories<User> userService;
        public UserService(IGenericReopsitories<User> userService)
        {
            this.userService = userService;
            
        }
        #endregion

        #region Login
        

        #endregion

        #region Dispose
        public void Dispose()
        {
            userService?.Dispose();
        }
        #endregion

    }
}

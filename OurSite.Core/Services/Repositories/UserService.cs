using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
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


        #region singup
        public async Task<singup> SingUp(ReqSingupUserDto userDto)
        {
            User user = new User();
           await userService.AddEntity(user);
           await userService.SaveEntity();

            
        }
        #endregion


        public Task UpDate(ReqUpdateUserDto userdto)
        {
            throw new NotImplementedException();
        }

        public Task ViewProfile(long id)
        {
            throw new NotImplementedException();
        }
        

    }
}

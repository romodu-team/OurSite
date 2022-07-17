using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ContactWithUs;
using OurSite.Core.DTOs.ContactWithUsDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.ContactWithUs;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class ContactWithUsService : IContactWithUsService
    {
        #region constructor
        private readonly IGenericReopsitories<ContactWithUs> contactWithUsRepo;
        public ContactWithUsService(IGenericReopsitories<ContactWithUs> contactWithUsRepo)
        {
            this.contactWithUsRepo = contactWithUsRepo;
        }
        #endregion

        #region Send ContactWithUs Form
        public async Task<bool> SendContactForm(ContactWithUsDto contactWithUsDto)
        {
            ContactWithUs form1 = new ContactWithUs()
            {
                UserFirstName = contactWithUsDto.UserFirstName,
                UserLastName = contactWithUsDto.UserLastName,
                UserEmail = contactWithUsDto.UserEmail,
                UserPhoneNumber = contactWithUsDto.UserPhoneNumber,
                Expration = contactWithUsDto.Expration,
            };
            try
            {
                await contactWithUsRepo.AddEntity(form1);
                await contactWithUsRepo.SaveEntity();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region See All ContactWithUs Form

        public async Task<ResFilterContactWithUsDto> GetAllContactWithUs(ReqFilterContactWithUsDto filter)
        {

            var contactWithUsQuery = contactWithUsRepo.GetAllEntity();
            var count = (int)Math.Ceiling(contactWithUsQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await contactWithUsRepo.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(x => new GetAllContactWithUsDto { UserFirstName = x.UserFirstName, UserLastName = x.UserLastName, UserEmail = x.UserEmail, UserPhoneNumber = x.UserPhoneNumber }).ToListAsync();    //use the genric interface options and save values in variable
            var result = new ResFilterContactWithUsDto();
            result.SetPaging(pager);
            return result.SetContactWithUses(list);

        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            contactWithUsRepo.Dispose();
        }
        #endregion
    }
}

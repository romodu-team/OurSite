using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
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

        #region Dispose
        public void Dispose()
        {
            contactWithUsRepo.Dispose();
        }
        #endregion
    }
}

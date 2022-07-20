using OurSite.Core.DTOs.InvoiceDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.Factor;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class InvoiceService : IInvoiceService
    {

        #region Constructor
        private readonly IGenericReopsitories<Invoice> invoiceRepo;
        public InvoiceService(IGenericReopsitories<Invoice> invoiceRepo)
        {
            this.invoiceRepo = invoiceRepo;
        }
        #endregion


        #region Create Factor
        public async Task<ResInvoiceDto> createFactor(InvoiceDto invoiceDto)
        {
            Invoice createInvoice = new Invoice()
            {
                Title = invoiceDto.InvoiceTitle,
                Description = invoiceDto.InvoiceDescription,
                Price = invoiceDto.InvoicePrice,
                UserEmail = invoiceDto.UserEmail, 
                UserId = invoiceDto.UserId,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                IsRemove = invoiceDto.IsRemove
            };
            try
            {
                await invoiceRepo.AddEntity(createInvoice);
                await invoiceRepo.SaveEntity();
                return new ResInvoiceDto { resInvoice = ResInvoice.Success };
            }
            catch
            {
                return new ResInvoiceDto { resInvoice = ResInvoice.Failed };
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            invoiceRepo.Dispose();
        }
        #endregion

    }
}

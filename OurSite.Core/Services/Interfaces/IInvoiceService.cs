using OurSite.Core.DTOs.InvoiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface IInvoiceService : IDisposable
    {
        Task<ResInvoiceDto> createFactor(InvoiceDto invoiceDto);
    }
}

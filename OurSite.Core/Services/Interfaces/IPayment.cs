using System;
using OurSite.Core.DTOs.Payment;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;

namespace OurSite.Core.Services.Interfaces.Projecta
{
    public interface IPayment : IDisposable
    {
        Task<ResProject> CreatePayment(CreatePaymentDto paydto);
        Task<GetPayment> GetPayment(long PayId);
        Task<ResFilterPayDto> GetAllPayments(ReqFilterPayDto filter);
        Task<ResProject> EditPay(EditPayDto Paydto);

    }
}


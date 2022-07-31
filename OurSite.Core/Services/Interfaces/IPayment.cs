﻿using System;
using OurSite.Core.DTOs.Payment;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Interfaces.Projecta
{
    public interface IPayment : IDisposable
    {
        Task<ResProject> CreatePayment(CreatePaymentDto paydto, long AdminId);
        Task<GetPayment> GetPayment(long PayId);
        Task<ResFilterPayDto> GetAllProject(ReqFilterPayDto filter);
        Task<ResProject> EditPay(EditPayDto Paydto);

    }
}

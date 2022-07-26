using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Payment;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.DataLayer.Entities.Payments;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Repositories
{
    public class PaymentService : IPayment
    {
        #region Cus&Dis
        private IGenericReopsitories<Payment> PaymentRepositories;
        public PaymentService(IGenericReopsitories<Payment> PaymentRepositories)
        {
            this.PaymentRepositories = PaymentRepositories;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<CreatProjectDto.ResProject> EditPay(EditPayDto Paydto)
        {
            throw new NotImplementedException();
        }
        #endregion


        public Task<CreatProjectDto.ResProject> EditProject(EditPayDto Paydto)
        {
            throw new NotImplementedException();
        }

        public Task<ResFilterPayDto> GetAllProject(ReqFilterPayDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetPayment> GetPayment(long PayId)
        {
            throw new NotImplementedException();
        }

        #region creat payment by admin (use in admin payment controller)
        public async Task<ResProject> CreatePayment(CreatePaymentDto paydto, long AdminId)
        {
            var admin = await PaymentRepositories.GetAllEntity().AnyAsync(x => x.AdminId == AdminId);
            if (admin is true)
            {
                if (!string.IsNullOrWhiteSpace(paydto.Titel) && !string.IsNullOrWhiteSpace(paydto.Price))
                {
                    Payment pay = new Payment()
                    {
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        Description = paydto.Description,
                        IsRemove = false,
                        Price = paydto.Price,
                        status = paydto.status,
                        Titel = paydto.Titel,
                    };

                    try
                    {
                        PaymentRepositories.AddEntity(pay);
                        PaymentRepositories.SaveEntity();
                        return ResProject.Success; //pay added

                    }
                    catch (Exception ex)
                    {
                        return ResProject.Faild; //pay add failed
                    }
                }
                return ResProject.InvalidInput; //titel and price is null
            }
            return ResProject.Error; //admin didnt find

        }
    }
    #endregion

}


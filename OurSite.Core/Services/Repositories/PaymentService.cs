using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.Payment;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.Payments;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Repositories
{
    public class PaymentService : IPayment
    {
        #region Cus&Dis
        private IGenericReopsitories<Payment> PaymentRepositories;
        private IGenericReopsitories<Admin> AdminRepositories;
        private IGenericReopsitories<Project> ProjectRepositories;
        public PaymentService(IGenericReopsitories<Payment> PaymentRepositories, IGenericReopsitories<Admin> AdminRepositories, IGenericReopsitories<Project> ProjectRepositories)
        {
            this.PaymentRepositories = PaymentRepositories;
            this.AdminRepositories = AdminRepositories;
            this.ProjectRepositories = ProjectRepositories;
        }

        public void Dispose()
        {
            PaymentRepositories.Dispose();
        }
        #endregion

        #region Edit
        public async Task<ResProject> EditPay(EditPayDto Paydto)
        {
            var res = await PaymentRepositories.GetAllEntity().AnyAsync(x => x.Id == Paydto.PayId);
            if (res is true)
            {
                var pay = await PaymentRepositories.GetEntity(Paydto.PayId);
                if (!string.IsNullOrWhiteSpace(Paydto.Titel))
                    pay.Titel = Paydto.Titel;
                if (Paydto.status is not null)
                    pay.status = Paydto.status;
                if (!string.IsNullOrWhiteSpace(Paydto.Price))
                    pay.Price = Paydto.Price;
                if (!string.IsNullOrWhiteSpace(Paydto.Description))
                    pay.Description = Paydto.Description;


                try
                {
                    PaymentRepositories.UpDateEntity(pay);
                    PaymentRepositories.SaveEntity();
                    return ResProject.Success;
                }
                catch (Exception ex)
                {
                    return ResProject.Faild;
                }
            }
            return ResProject.NotFound;
        }
        #endregion

        #region Get all project
        public async Task<ResFilterPayDto> GetAllPayments(ReqFilterPayDto filter)
        {
            var PayQuery = PaymentRepositories.GetAllEntity().AsQueryable();
            switch (filter.CreatDateOrderBy)
            {
                case PayDateOrderBy.CreateDateAsc:
                    PayQuery = PayQuery.OrderBy(x => x.CreateDate);
                    break;
                case PayDateOrderBy.CreateDateDec:
                    PayQuery = PayQuery.OrderByDescending(x => x.CreateDate);
                    break;
                case PayDateOrderBy.DatePayAsc:
                    PayQuery = PayQuery.OrderBy(x => x.DatePay);
                    break;
                case PayDateOrderBy.DatePayDec:
                    PayQuery = PayQuery.OrderByDescending(x => x.DatePay);
                    break;
                default:
                    break;
            }

            switch (filter.StatusPay)
            {
                case StatusPay.Success:
                    PayQuery = PayQuery.Where(x => x.status == StatusPay.Success);
                    break;
                case StatusPay.Faild:
                    PayQuery = PayQuery.Where(x => x.status == StatusPay.Faild);
                    break;
                case StatusPay.Pending:
                    PayQuery = PayQuery.Where(x => x.status == StatusPay.Pending);
                    break;
                default:
                    break;
            }

            switch (filter.RemoveFilter)
            {
                case PayRemoveFilter.Deleted:
                    PayQuery = PayQuery.Where(x => x.IsRemove == true);
                    break;
                case PayRemoveFilter.NotDeleted:
                    PayQuery = PayQuery.Where(x => x.IsRemove == false);
                    break;
                case PayRemoveFilter.All:
                    PayQuery = PaymentRepositories.GetAllEntity();
                    break;
                default:
                    break;

            }
            if (!string.IsNullOrWhiteSpace(filter.UserName))
                PayQuery = PayQuery.Include(x => x.User).Where(x => x.User.UserName.Contains(filter.UserName.ToLower()));

            var count = (int)Math.Ceiling(PayQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await PayQuery.Paging(pager).Include(x => x.User).Select(x => new GetAllPayDto { DatePay = x.DatePay, Id = x.Id, status = (StatusPay)x.status, Username = x.User.UserName, ProId = x.ProId, Price = x.Price }).ToListAsync();

            var result = new ResFilterPayDto();
            result.SetPaging(pager);
            return result.SetPay(list);

        }

        #endregion

        #region View payment
        public async Task<GetPayment> GetPayment(long PayId)
        {
            var payment = await PaymentRepositories.GetEntity(PayId);
            if (payment is not null)
            {
                GetPayment getPayment = new GetPayment()
                {
                    Titel = payment.Titel,
                    status = (StatusPay)payment.status,
                    Description = payment.Description,
                    Price = payment.Price,
                    ProId = payment.ProId,
                    UserId = payment.UserId
                };
                return getPayment;
            }
            return null;
        }
        #endregion

        #region create payment by admin (use in admin payment controller)
        public async Task<ResProject> CreatePayment(CreatePaymentDto paydto)
        {
            var project = await ProjectRepositories.GetEntity(paydto.ProId);
            if (project is not null)
            {
                if (!string.IsNullOrWhiteSpace(paydto.Titel) && !string.IsNullOrWhiteSpace(paydto.Price))
                {
                    Payment pay = new Payment()
                    {

                        Description = paydto.Description,
                        IsRemove = false,
                        Price = paydto.Price,
                        status = paydto.status,
                        Titel = paydto.Titel,
                        ProId = paydto.ProId,
                        UserId = project.UserId
                    };

                    try
                    {
                        await PaymentRepositories.AddEntity(pay);
                        await PaymentRepositories.SaveEntity();
                        return ResProject.Success; //pay added

                    }
                    catch (Exception ex)
                    {
                        return ResProject.Faild; //pay add failed
                    }
                }
                return ResProject.InvalidInput; //titel and price is null
            }
            return ResProject.NotFound;
        }
    }
    #endregion

}


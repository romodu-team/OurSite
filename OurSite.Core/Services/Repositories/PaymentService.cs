using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.Payment;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.Payments;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using OurSite.Core.Utilities.Extentions;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;

namespace OurSite.Core.Services.Repositories
{
    public class PaymentService : IPayment
    {
        #region Cus&Dis
        private IGenericRepository<Payment> PaymentRepositories;
        private IGenericRepository<Admin> AdminRepositories;
        private IGenericRepository<Project> ProjectRepositories;
        private INotificationService _notificationService;
        private IMailService _mailService;
        private IUserService _userService;
        public PaymentService(IUserService userService, IGenericRepository<Payment> PaymentRepositories, IGenericRepository<Admin> AdminRepositories, IGenericRepository<Project> ProjectRepositories, IMailService mailService, INotificationService notificationService)
        {
            _userService = userService;
            this.PaymentRepositories = PaymentRepositories;
            this.AdminRepositories = AdminRepositories;
            this.ProjectRepositories = ProjectRepositories;
            _mailService = mailService;
            _notificationService = notificationService;
        }

        public void Dispose()
        {
            PaymentRepositories.Dispose();
            AdminRepositories.Dispose();
            ProjectRepositories.Dispose();
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
            var list = await PayQuery.Paging(pager).Include(x => x.User).Select(x => new GetAllPayDto { DatePay = x.DatePay != null?x.DatePay.Value.PersianDate():null , PayId = x.Id, status = (StatusPay)x.status,IsRemove = x.IsRemove, Username = x.User.UserName, ProId = x.ProId, Price = x.Price }).ToListAsync();

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
                        var account =await _userService.GetUserById(project.UserId);
                        await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = account.UUID, Message = $"فاکتور جدید برای پروژه {project.Name} ایجاد شد . شماره فاکتور {pay.Id}" });
                        if (account.Email != null)
                            await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = account.Email, Subject = $"فاکتور جدید برای پروژه {project.Name} ایجاد شد . شماره فاکتور {pay.Id}", Body = $"{pay.Id}\t{pay.Titel}\t{pay.Description}\t{pay.Price}\t{pay.status.Value}" });
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
        #endregion

        #region Delete payment by admin
        public async Task<ResProject> DeletePayment(long ProId, bool IsAdmin)
        {
            var pay = await PaymentRepositories.GetEntity(ProId);
            if(pay is not null)
            {
                if (IsAdmin)
                {
                    var IsRemove = await PaymentRepositories.DeleteEntity(pay.Id);
                    if(IsRemove is true)
                    {
                        await PaymentRepositories.SaveEntity();
                        return ResProject.Success;
                    }
                    else
                    {
                        return ResProject.Faild;
                    }
                }
            }
            return ResProject.Error;
        }
        #endregion

        public async Task<List<Payment>> GetPayments(long ProjectId)
        {
            var payment = await PaymentRepositories.GetAllEntity().Where(x => x.ProId == ProjectId).ToListAsync();
            return payment;
        }

    }



}


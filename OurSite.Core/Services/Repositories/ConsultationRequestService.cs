using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ConsultationRequestDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        #region constructor
        private readonly IGenericReopsitories<ConsultationRequest> consultationRequestRepo;
        private readonly IGenericReopsitories<CheckBoxs> checkBoxsRepo;
        private readonly IGenericReopsitories<ItemSelected> itemSelectedRepo;
        public ConsultationRequestService(IGenericReopsitories<ItemSelected> itemSelectedRepo, IGenericReopsitories<CheckBoxs> checkBoxsRepo, IGenericReopsitories<ConsultationRequest> consultationRequestRepo)
        {
            this.consultationRequestRepo = consultationRequestRepo;
            this.checkBoxsRepo = checkBoxsRepo;
            this.itemSelectedRepo = itemSelectedRepo;
        }
        #endregion


        public async Task<bool> SendConsultationForm(ConsultationRequestDto consultationRequestDto)
        {
            bool flag = true;
            ConsultationRequest request = new ConsultationRequest()
            {
                UserFullName = consultationRequestDto.UserFullName,
                UserEmail = consultationRequestDto.UserEmail,
                UserPhoneNumber = consultationRequestDto.UserPhoneNumber,
                Expration = consultationRequestDto.Expration,
                SubmittedFileName = consultationRequestDto.SubmittedFileName,
            };
            try
            {
                await consultationRequestRepo.AddEntity(request);
                await consultationRequestRepo.SaveEntity();
            }
            catch (Exception)
            {

                flag = false;
                return flag;
            }
           

            foreach (var item in consultationRequestDto.ItemSelecteds)
            {
                if (item.IsCheck == true)
                {
                    if (await checkBoxsRepo.GetEntity(item.CheckBoxId) != null)
                    {
                        var selectedItem = new ItemSelected()
                        {
                            CheckBoxId = item.CheckBoxId,
                            ConsultationFormId = request.Id
                            
                        };
                        await itemSelectedRepo.AddEntity(selectedItem);
                    }
                }
            }
            try
            {
                await itemSelectedRepo.SaveEntity();
                
                flag= true;
                return flag;
            }
            catch
            {
                flag= false;
                return flag;
            }
        }



        #region See All ConsultationRequest Form

        public async Task<ResFilterConsultationRequestDto> GetAllConsultationRequest(ReqFilterConsultationRequestDto filter)
        {

            var consultationRequestQuery = consultationRequestRepo.GetAllEntity();
            var count = (int)Math.Ceiling(consultationRequestQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await consultationRequestRepo.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(x => new GetAllConsultationRequestDto { UserFullName = x.UserFullName, UserEmail = x.UserEmail, UserPhoneNumber = x.UserPhoneNumber }).ToListAsync();    //use the genric interface options and save values in variable
            var result = new ResFilterConsultationRequestDto();
            result.SetPaging(pager);
            return result.SetConsultationRequests(list);

        }

        #endregion


        #region Dispose
        public void Dispose()
        {
            consultationRequestRepo.Dispose();
        }
        #endregion
    }
}

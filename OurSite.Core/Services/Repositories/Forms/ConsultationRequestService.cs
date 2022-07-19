using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
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
        public ConsultationRequestService(IGenericReopsitories<ConsultationRequest> consultationRequestRepo)
        {
            this.consultationRequestRepo = consultationRequestRepo;
        }
        #endregion


        public async Task<bool> SendConsultationForm(ConsultationRequestDto consultationRequestDto)
        {
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
            consultationRequestRepo.Dispose();
        }
        #endregion
    }
}

using OurSite.Core.DTOs.ConsultationRequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface IConsultationRequestService : IDisposable
    {
        Task<bool> SendConsultationForm(ConsultationRequestDto consultationRequestDto,string? SubmittedFileName);

        Task<ResFilterConsultationRequestDto> GetAllConsultationRequest(ReqFilterConsultationRequestDto filter);

        Task<GetConsulationFormDto> GetConsulationForm(long ConsultationFormId);
        Task<bool> ChangeReadStatus(long ConsultationFormId);
    }
}

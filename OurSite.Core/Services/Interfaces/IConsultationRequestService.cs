using OurSite.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface IConsultationRequestService : IDisposable
    {
        Task<bool> SendConsultationForm(ConsultationRequestDto consultationRequestDto);

    }
}

using OurSite.Core.DTOs;
using OurSite.Core.DTOs.MailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestDto mailRequest);

        Task<bool> SendResetPasswordEmailAsync(SendEmailDto request);

        Task<bool> SendActivationCodeEmail(SendEmailDto request);
    }
}

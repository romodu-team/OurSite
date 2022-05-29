using OurSite.Core.DTOs;
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

        Task<bool> SendResetPasswordEmailAsync(ResetPassEmailDto request);
    }
}

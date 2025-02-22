using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.EmailServices
{
    public interface IEmailService
    {
        Task SendResetLinkAsync(string email, string token);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendMailAsync(string to, string subject, string message);

        public Task<bool> SendHtmlEmailAsync(string to, string subject, string templateName, params string[] parameters);
    }
}

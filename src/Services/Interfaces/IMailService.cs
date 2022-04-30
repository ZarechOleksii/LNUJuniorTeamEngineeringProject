using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendMailAsync(string to, string message, string subject);
    }
}

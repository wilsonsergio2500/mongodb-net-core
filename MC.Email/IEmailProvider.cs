using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Email
{
    public interface IEmailProvider
    {
        Task<bool> SendEmailAsPlain(string To, string Content);
        Task<bool> SendEmailAsHtml(string To, string Content);
    }
}

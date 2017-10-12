using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Email.Models
{
    public class EmailClientConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string FromAddress { get; set; }

        public string Subject { get; set; }
    }
}

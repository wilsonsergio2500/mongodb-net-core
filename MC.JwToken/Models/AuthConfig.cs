using System;
using System.Collections.Generic;
using System.Text;

namespace MC.JwToken.Models
{
    public class AuthConfig
    {
        public string secret { get; set; }
        public int hours { get; set; }

        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string tokenKey { get; set; }
    }
}

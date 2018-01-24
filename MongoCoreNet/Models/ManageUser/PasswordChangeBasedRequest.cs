using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Models
{
    public class PasswordChangeBasedRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

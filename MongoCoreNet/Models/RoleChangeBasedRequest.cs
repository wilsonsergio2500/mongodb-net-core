using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Models
{
    public class RoleChangeBasedRequest
    {
        public string email { get; set; }
        public int role { get; set; }
    }
}

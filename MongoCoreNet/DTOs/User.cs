using MC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mdls = MC.Models;

namespace MongoCoreNet.DTOs
{
    public class User
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string UserName { get; set; }

        public string Bio { get; set; }

        public List<Strength> Strengths { get; set; }

        public string JobTitle { get; set; }

        public Mdls.RoleType Role { get; set; }

        public string Image { get; set; }
    }
}

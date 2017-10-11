using MC.Models.attributes;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Roles)]
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public RoleType RoleTypeId { get; set; } 
    }
}

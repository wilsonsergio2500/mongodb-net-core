using MC.Models.attributes;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Users)]
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        public List<Strength> Strengths { get; set; }
        public string Password { get; set; }
        public string EncryptionKey { get; set; }
        public RoleType Role { get; set; }
        public string Image { get; set; }
    }
}

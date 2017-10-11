using MC.Models.attributes;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Invites)]
    public class Invite : BaseEntity
    {
        public string Email { get; set; }

        public RoleType ParticipationRoleType { get; set; }

        public InviteStatus InviteStatus { get; set; }

    }
}

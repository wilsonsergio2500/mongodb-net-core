using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mdls = MC.Models;

namespace MongoCoreNet.DTOs
{
    public class InviteCompletion
    {
        public Mdls.User User { get; set; }
        public string InvitationId { get; set; }
    }
}

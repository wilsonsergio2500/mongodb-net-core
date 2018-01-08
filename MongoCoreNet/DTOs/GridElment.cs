using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mdls = MC.Models;

namespace MongoCoreNet.DTOs
{
    public class GridElment
    {
        public Mdls.Milestone Milestone { get; set; }
        public User User { get; set; }
        public Models.enums.LikeType Like { get; set; }

        public bool Self { get; set; }
    }
}

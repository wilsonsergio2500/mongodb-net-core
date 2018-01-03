using MC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Models.Profile
{
    public class BioEdit
    {
        public string Bio { get; set; }

        public string JobTitle { get; set; }

        public List<Strength> Strengths { get; set; }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mdls = MC.Models;

namespace MongoCoreNet
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTOs.User, Mdls.User>().ReverseMap();

        }
    }
}

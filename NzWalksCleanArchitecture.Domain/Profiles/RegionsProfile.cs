using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzWalksCleanArchitecture.Domain.Profiles
{
    public class RegionsProfile:Profile
    {
        public RegionsProfile() 
        {
            CreateMap<Models.Region, Dtos.Region>()
            .ReverseMap();
        }
    }
}

using AutoMapper;
using NzWalksCleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzWalksCleanArchitecture.Domain.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.Walk, Dtos.Walk>()
                .ReverseMap();

            CreateMap<Models.WalkDifficulty, Dtos.WalkDifficulty>()
                .ReverseMap();
        }
    }
}


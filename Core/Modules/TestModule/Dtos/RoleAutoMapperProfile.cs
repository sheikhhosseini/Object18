using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;

namespace Core.Modules.TestModule.Dtos
{
    public class RoleAutoMapperProfile : Profile
    {
        public RoleAutoMapperProfile()
        {
            CreateMap<RoleCreateDto, Role>(MemberList.Destination);


            CreateMap<Role, RoleCreateDto>(MemberList.Destination);
        }
    }
}

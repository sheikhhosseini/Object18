using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;

namespace Core.Modules.TestModule.Dtos
{
    public class RoleCreateDto
    {
        public string RoleTitle { get; set; }

        public string RoleDescription { get; set; }
    }

    
}

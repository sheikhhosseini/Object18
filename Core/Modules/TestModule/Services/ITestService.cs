using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Modules.TestModule.Dtos;
using Data.Models;

namespace Core.Modules.TestModule.Services
{
    public interface ITestService
    {
        Task AddTest(RoleCreateDto createDto);
    }
}

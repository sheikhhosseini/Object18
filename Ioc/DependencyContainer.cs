using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.TestModule.Dtos;
using Core.Modules.TestModule.Services;
using Core.Repository;

namespace Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection service)
        {
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<ITestService, TestService>();

            #region AutoMapper Configs
            service.AddAutoMapper(typeof(RoleAutoMapperProfile));



            #endregion
        }
    }
}

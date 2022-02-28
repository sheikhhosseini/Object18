using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection service)
        {
            ////Application Layer
            //service.AddScoped<ICourseService, CourseService>();
            //service.AddScoped<IUserService, UserService>();

            ////Infra Data Layer
            //service.AddScoped<ICourseRepository, CourseReository>();
            //service.AddScoped<IUserRepository, UserRepository>();
        }
    }
}

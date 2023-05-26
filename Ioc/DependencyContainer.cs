using Core.Modules.AccountModule.Dtos;
using Core.Modules.AccountModule.Services;
using Core.Modules.MemberModule.Services;
using Core.Modules.UserModule.Services;
using Core.Shared.DataTable;
using Microsoft.Extensions.DependencyInjection;
using Core.Shared.Email;

namespace Ioc;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection service)
    {
        service.AddScoped<IAccountService, AccountService>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IMemberService, MemberService>();

        #region Email
        service.AddScoped<IMailSender, SendEmail>();
        service.AddScoped<IViewRenderService, RenderViewToString>();
        #endregion

        #region AutoMapper Configs
        service.AddAutoMapper(typeof(AccountProfile));
        #endregion

        service.AddScoped<IDataTableService, DataTableService>();
    }
}
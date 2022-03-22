using Core.Modules.Account.Dtos;
using Core.Modules.Account.Services;
using Microsoft.Extensions.DependencyInjection;
using Core.Repository;
using Core.Shared.Email;

namespace Ioc;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection service)
    {
        service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        service.AddScoped<IAccountService, AccountService>();

        #region Email
        service.AddScoped<IMailSender, SendEmail>();
        service.AddScoped<IViewRenderService, RenderViewToString>();
        #endregion

        #region AutoMapper Configs
        service.AddAutoMapper(typeof(AccountProfile));
        #endregion
    }
}
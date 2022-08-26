using AutoMapper;
using Core.Modules.AccountModule.Dtos;
using Core.Modules.AccountModule.Results;
using Core.Shared.Email;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.AccountModule.Services;

public class AccountService : IAccountService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMailSender _mailSender;
    private readonly IViewRenderService _renderViewService;

    public AccountService(MainDbContext dbContext, IMapper mapper, IMailSender mailSender, IViewRenderService renderViewService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mailSender = mailSender;
        _renderViewService = renderViewService;
    }

    public async Task<RegisterDto> RegisterUser(RegisterDto inputDto, bool sendEmail = true)
    {
        User newUser = _mapper.Map<User>(inputDto);
        newUser.ActiveCode = MyUniqCode.GenerateActiveCode();
        newUser.UserImage = "Default.jpg";
        newUser.Password.EncodePasswordMd5();

        await _dbContext.AddEntityAsync(newUser, false);
        await _dbContext.SaveChangesAsync();

        #region Send Email
        if (sendEmail)
        {
            var body = await _renderViewService.RenderToStringAsync("Email/ActiveAccount", newUser);
            _mailSender.Send(newUser.Email, "فعال سازی حساب کاربری", body);
        }
        #endregion

        return inputDto;
    }

    public async Task<bool> IsUserExist(string email)
    {
        return await _dbContext.Set<User>()
            .AnyAsync(u => u.Email == email);
    }

    public async Task<ActiveAccountResult> ActiveAccount(string activeCode)
    {
        var user = await _dbContext.Set<User>()
            .SingleOrDefaultAsync(u => u.ActiveCode == activeCode);

        if (user != null)
        {
            if (user.IsActive == false)
            {
                user.IsActive = true;
                user.ActiveCode = MyUniqCode.GenerateActiveCode();
                _dbContext.UpdateEntityAsync(user);
                await _dbContext.SaveChangesAsync();
                return ActiveAccountResult.Success;
            }
            return ActiveAccountResult.AlreadyActive;
        }
        return ActiveAccountResult.Failed;
    }

    public async Task<LoginResult> LoginUser(LoginDto loginDto)
    {
        var user = await _dbContext.Set<User>().SingleOrDefaultAsync(
            u => u.Email == loginDto.Email
            && u.Password == loginDto.Password.EncodePasswordMd5());

        if (user is null)
            return new LoginResult { Status = LoginStatus.Failed, User = null };

        if (!user.IsActive)
            return new LoginResult { Status = LoginStatus.NotActivated, User = user };

        return new LoginResult { Status = LoginStatus.Success, User = user };
    }
}
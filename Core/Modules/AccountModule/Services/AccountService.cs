using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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

        await _dbContext.AddEntityAsync(newUser);
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
                _dbContext.UpdateEntity(user);
                await _dbContext.SaveChangesAsync();
                return ActiveAccountResult.Success;
            }
            return ActiveAccountResult.AlreadyActive;
        }
        return ActiveAccountResult.Failed;
    }

    public async Task<LoginResult> LoginUser(LoginDto loginDto)
    {
        var user = await _dbContext.Set<User>()
            .AsNoTracking()
            .SingleOrDefaultAsync(u => 
                u.Email == loginDto.Email 
                && u.Password == loginDto.Password.EncodePasswordMd5());

        if (user is null)
            return new LoginResult { Status = LoginStatus.Failed, User = null };

        if (!user.IsActive)
            return new LoginResult { Status = LoginStatus.NotActivated, User = user };

        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email)
        };

        var roleIds = await _dbContext.GetAsNoTrackingQuery<UserRole>()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleId).ToArrayAsync();

        var permissions = await _dbContext.GetAsNoTrackingQuery<RolePermission>()
            .Where(x => roleIds.Contains(x.RoleId))
            .Select(x => x.Permission.Name).ToListAsync();

        permissions.ForEach(x=>claims.Add(new("permission", x)));

        return new LoginResult { Status = LoginStatus.Success, User = user , Claims = claims};
    }

    public async Task<bool> HasPermission(long userId, string permissionName)
    {
        var roleIds = await _dbContext.GetAsNoTrackingQuery<UserRole>()
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId).ToArrayAsync();

        var permissions = await _dbContext.GetAsNoTrackingQuery<RolePermission>()
            .Where(x => roleIds.Contains(x.RoleId))
            .Select(x => x.Permission.Name).ToListAsync();

        return permissions.Any(x => x == permissionName);
    }
}
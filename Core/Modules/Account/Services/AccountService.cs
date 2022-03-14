using AutoMapper;
using Core.Modules.Account.Dtos;
using Core.Modules.Account.ResultDtos;
using Core.Repository;
using Core.Shared.Email;
using Core.Shared.Tools;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Account.Services;

public class AccountService : IAccountService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IMailSender _mailSender;
    private readonly IViewRenderService _renderViewService;

    public AccountService(IGenericRepository<User> userRepository, IMapper mapper, IMailSender mailSender, IViewRenderService renderViewService)
    {
        _userRepository = userRepository;
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

        await _userRepository.AddEntity(newUser);
        await _userRepository.SaveChangesAsync();

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
        return await _userRepository.GetEntitiesQuery()
            .AnyAsync(u => u.Email == email);
    }

    public async Task<ActiveAccountResultDto> ActiveAccount(string activeCode)
    {
        var user = await _userRepository.GetEntitiesQuery()
            .SingleOrDefaultAsync(u => u.ActiveCode == activeCode);

        if (user != null)
        {
            if (user.IsActive == false)
            {
                user.IsActive = true;
                user.ActiveCode = MyUniqCode.GenerateActiveCode();
                user.LastUpdateDate = DateTime.Now;
                await _userRepository.SaveChangesAsync();
                return ActiveAccountResultDto.Success;
            }
            return ActiveAccountResultDto.AlreadyActive;
        }
        return ActiveAccountResultDto.Failed;
    }

    
}
using AutoMapper;
using Core.Modules.Account.Dtos;
using Core.Repository;
using Core.Shared.Tools;
using Data.Models;

namespace Core.Modules.Account.Services;

public class AccountService : IAccountService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public AccountService(IGenericRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<RegisterDto> RegisterUser(RegisterDto inputDto)
    {
        User newUser = _mapper.Map<User>(inputDto);
        newUser.ActiveCode = MyUniqCode.ActiveCode();
        newUser.UserImage = "Default.jpg";
        newUser.Password.EncodePasswordMd5();

        await _userRepository.AddEntity(newUser);
        await _userRepository.SaveChangesAsync();
        return inputDto;
    }

    //public void Dispose()
    //{
    //    _userRepository?.Dispose();
    //}
}
using AutoMapper;
using Data.Models;

namespace Core.Modules.UserModule.Dtos;

public class UserUpdateDto : UserCreateDto
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string Image { get; set; }
}

public class UserUpdateDtoProfile : Profile
{
    public UserUpdateDtoProfile()
    {
        CreateMap<User, UserUpdateDto>(MemberList.Destination)
            .ForMember(dto => dto.ImageFile, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dto => dto.UserImage, opt => opt.Ignore());
    }
}
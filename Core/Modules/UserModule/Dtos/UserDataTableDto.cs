using AutoMapper;
using Data.Models;

namespace Core.Modules.UserModule.Dtos;

public class UserDataTableDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public bool IsActive { get; set; }
    
    public int Row { get; set; }
}

public class UserDataTableDtoProfile : Profile
{
    public UserDataTableDtoProfile()
    {
        CreateMap<User, UserDataTableDto>(MemberList.Destination).ReverseMap();
    }
}
using AutoMapper;
using Data.Models;

namespace Core.Modules.MemberModule.Dtos;

public class MemberDataTableDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public bool IsActive { get; set; }
    
    public int Row { get; set; }
}

public class MemberDataTableDtoProfile : Profile
{
    public MemberDataTableDtoProfile()
    {
        CreateMap<Member, MemberDataTableDto>(MemberList.Destination).ReverseMap();
    }
}
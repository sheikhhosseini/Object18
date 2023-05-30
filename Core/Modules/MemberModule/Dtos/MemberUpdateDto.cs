using AutoMapper;
using Data.Models;

namespace Core.Modules.MemberModule.Dtos;

public class MemberUpdateDto : MemberCreateDto
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; }
}

public class MemberUpdateDtoProfile : Profile
{
    public MemberUpdateDtoProfile()
    {
        CreateMap<Member, MemberUpdateDto>(MemberList.Destination)
            .ForMember(dto => dto.Image, opt => opt.Ignore())
            //.ForMember(dto => dto.ConcurrencyStamp, opt => opt.Ignore())
            .ReverseMap();
    }
}
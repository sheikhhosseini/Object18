using AutoMapper;
using Core.Shared.Tools;
using Data.Models;

namespace Core.Modules.AccountModule.Dtos;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        // From Dto To Model
        CreateMap<RegisterDto, User>(MemberList.Destination)
            .ForMember(model => model.FirstName, m =>
                m.MapFrom(dto => dto.FirstName.SanitizeText()))
            .ForMember(model => model.LastName, m =>
                m.MapFrom(dto => dto.LastName.SanitizeText()))
            .ForMember(model => model.Email, m =>
                m.MapFrom(dto => dto.Email.SanitizeText()))
            .ForMember(model => model.Password, m =>
            m.MapFrom(dto => dto.Password.EncodePasswordMd5()));
    }
}
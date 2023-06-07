using AutoMapper;
using Core.Shared.DataTable;
using Data.Models;

namespace Core.Modules.MemberModule.Dtos;

public class MemberDataTableDto : DataTableBaseDto
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public string KodMeli { get; set; }

    public string DateOfBirth { get; set; }

    public string RegisterDate { get; set; }

    public bool Gender { get; set; }

    public string Address { get; set; }

}

public class MemberDataTableDtoProfile : Profile
{
    public MemberDataTableDtoProfile()
    {
        CreateMap<Member, MemberDataTableDto>(MemberList.Destination).ReverseMap();
    }
}
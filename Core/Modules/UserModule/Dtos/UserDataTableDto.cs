using AutoMapper;
using Core.Shared.DataTable;
using Data.Models;

namespace Core.Modules.UserModule.Dtos;

public class UserDataTableDto : DataTableBaseDto
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

public class UserDataTableDtoProfile : Profile
{
    public UserDataTableDtoProfile()
    {
        CreateMap<User, UserDataTableDto>(MemberList.Destination).ReverseMap();
    }
}
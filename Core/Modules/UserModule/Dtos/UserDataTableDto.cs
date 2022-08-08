using AutoMapper;
using Core.Shared.Paging;
using Data.Models;

namespace Core.Modules.UserModule.Dtos;

public class MyDataTable<T> : BasePaging
{
    public List<T> Records { get; set; }

    public MyDataTable<T> SetPaging(BasePaging paging)
    {
        this.PageId = paging.PageId;
        this.PageCount = paging.PageCount;
        this.StartPage = paging.StartPage;
        this.EndPage = paging.EndPage;
        this.TakeEntity = paging.TakeEntity;
        this.SkipEntity = paging.SkipEntity;
        this.ActivePage = paging.ActivePage;
        return this;
    }

    public MyDataTable<T> SetList(List<T> records)
    {
        this.Records = records;
        return this;
    }
}


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
using AutoMapper;
using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Core.Modules.MemberModule.Services;

public class MemberService : IMemberService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;

    public MemberService(MainDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AdvanceDataTable<MemberDataTableDto>> GetDataTable(
        AdvanceDataTable<MemberDataTableDto> data
    )
    {
        var query = _dbContext.GetEntitiesAsNoTrackingQuery<Member>();

        foreach (var filter in data.Filters)
        {
            if (filter.KeyType is "text" or "number" && !string.IsNullOrEmpty(filter.KeyValue.First()))
            {
                query = query.ApplyFiltering($"{filter.KeyName} {filter.KeyOperator} {filter.KeyValue.First()}");
            }
            else if (filter.KeyType == "date" && !string.IsNullOrEmpty(filter.KeyValue.First()))
            {
                query = query.Where("string.Compare(" + filter.KeyName + ", @0)" + filter.KeyOperator + "0", filter.KeyValue.First());
            }
            else if (filter.KeyType == "list")
            {

            }
        }

        foreach (var sortOrder in data.SortOrder)
        {
            if (!string.IsNullOrEmpty(sortOrder.KeyName) && !string.IsNullOrEmpty(sortOrder.KeySort))
            {
                query = query.ApplyOrdering($"{sortOrder.KeyName} {sortOrder.KeySort}");
            }
        }

        return await GeneratePages(data, query);
    }

    public async Task<OperationResult<MemberUpdateDto>> Create(MemberCreateDto createDto)
    {
        var newMember = _mapper.Map<Member>(createDto);
        string imageName = await FileSaver.CreateImage(createDto.Image);
        newMember.Image = imageName;

        await _dbContext.AddEntityAsync(newMember);
        await _dbContext.SaveChangesAsync();

        return new OperationResult<MemberUpdateDto>
        {
            Message = "عضو جدید با موفقیت ایجاد شد",
            Type = OperationResultType.Single,
            Response = Response.Success
        };
    }

    public async Task<MemberUpdateDto> Update(MemberUpdateDto updateDto)
    {
        var existingMember = await _dbContext.GetEntitiesQuery<Member>()
            .Where(u => u.Id == 1)
            .SingleOrDefaultAsync();

        _mapper.Map(updateDto, existingMember);

        _dbContext.UpdateEntityAsync(existingMember);
        await _dbContext.SaveChangesAsync();

        var x = new OperationResult<MemberUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = "",
            Record = new MemberUpdateDto()
        };
        
        return null;
    }

    public async Task<MemberUpdateDto> Get(long id)
    {
        var existingMember = await _dbContext.GetEntitiesQuery<Member>()
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync();

        return _mapper.Map<MemberUpdateDto>(existingMember);
    }

    public async Task<OperationResult<MemberUpdateDto>> Delete(List<long> deleteDtos)
    {
        var users = await _dbContext.GetEntitiesQuery<Member>()
            .Where(u => deleteDtos.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            _dbContext.SoftRemoveEntity(user);
        }

        await _dbContext.SaveChangesAsync();

        return new OperationResult<MemberUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = $"'{users.Count}' کاربر با موفقیت حذف شد",
        };
    }

    private async Task<AdvanceDataTable<MemberDataTableDto>> GeneratePages(
        AdvanceDataTable<MemberDataTableDto> data,
        IQueryable<Member> query
    )
    {

        var pageCount = (int)Math.Ceiling(query.Count() / (double)data.TakeEntity);
        var pager = PageGenerator.Generate(pageCount, data.PageId, data.TakeEntity);
        var users = await query.Paging(pager).ToListAsync();

        var records = _mapper.Map<List<MemberDataTableDto>>(users);

        var rowNumber = pager.SkipEntity;
        foreach (var record in records)
        {
            record.Row = ++rowNumber;
        }

        var result = new AdvanceDataTable<MemberDataTableDto>
        {
            Records = records,
            PageId = pager.PageId,
            PageCount = pager.PageCount,
            StartPage = pager.StartPage,
            EndPage = pager.EndPage,
            TakeEntity = pager.TakeEntity,
            SkipEntity = pager.SkipEntity,
            ActivePage = pager.ActivePage,
        };

        return result;
    }
}
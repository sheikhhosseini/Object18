using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Core.Shared.DataTable;

namespace Core.Modules.MemberModule.Services;

public class MemberService : IMemberService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDataTableService _dataTable;

    public MemberService(
        MainDbContext dbContext,
        IMapper mapper,
        IDataTableService dataTable
    )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dataTable = dataTable;
    }

    public async Task<AdvanceDataTable<MemberDataTableDto>> GetDataTable(
        AdvanceDataTable<MemberDataTableDto> dataTableRequest
    )
    {
        return await _dataTable.GetDataTable(_dbContext.GetAsNoTrackingQuery<Member>(), dataTableRequest);
    }

    public async Task<OperationResult<MemberUpdateDto>> Create(MemberCreateDto createDto)
    {
        if (await IsKodMeliDuplicate(null, createDto.KodMeli))
        {
            return new OperationResult<MemberUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "کد ملی تکراری است."
            };
        }

        if (await IsMobileNumberDuplicate(null, createDto.MobileNumber))
        {
            return new OperationResult<MemberUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "شماره تلفن تکراری است."
            };
        }

        var newMember = _mapper.Map<Member>(createDto);
        string imageName = await FileSaver.CreateImage(createDto.ImageFile, nameof(Member));
        newMember.Image = imageName;

        await _dbContext.AddEntityAsync(newMember);
        await _dbContext.SaveChangesAsync();

        return new OperationResult<MemberUpdateDto>
        {
            Message = "عضو جدید با موفقیت ایجاد شد.",
            Type = OperationResultType.Single,
            Response = Response.Success
        };
    }

    public async Task<OperationResult<MemberUpdateDto>> Update(MemberUpdateDto updateDto)
    {
        if (await IsKodMeliDuplicate(updateDto.Id, updateDto.KodMeli))
        {
            return new OperationResult<MemberUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "کد ملی تکراری است."
            };
        }

        if (await IsMobileNumberDuplicate(updateDto.Id, updateDto.MobileNumber))
        {
            return new OperationResult<MemberUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "شماره تلفن تکراری است."
            };
        }

        var existingMember = await _dbContext.Members
            .Where(member => member.Id == updateDto.Id)
            .SingleOrDefaultAsync();

        if (existingMember == null) return null;

        _mapper.Map(updateDto, existingMember);

        existingMember.Image = await FileSaver.UpdateImage(updateDto.ImageFile, existingMember.Image, nameof(Member));

        _dbContext.UpdateEntity(existingMember);

        _dbContext.Entry(existingMember).Property(member => member.ConcurrencyStamp).OriginalValue =
            updateDto.ConcurrencyStamp;

        existingMember.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _dbContext.SaveChangesAsync();

        return new OperationResult<MemberUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = "عضو با موفقیت ویرایش شد."
        };
    }

    public async Task<MemberUpdateDto> Get(long id)
    {
        return await _dbContext.Members
            .Where(member => member.Id == id)
            .ProjectTo<MemberUpdateDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<OperationResult<MemberUpdateDto>> Delete(List<long> deleteDtos)
    {
        var members = await _dbContext.Members
            .Where(u => deleteDtos.Contains(u.Id))
            .ToListAsync();

        foreach (var member in members)
        {
            _dbContext.SoftRemoveEntity(member);
        }

        await _dbContext.SaveChangesAsync();

        return new OperationResult<MemberUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = $"'{members.Count}' عضو با موفقیت حذف شد.",
        };
    }

    public async Task<List<SelectItemDto>> SelectItems()
    {
        return await _dbContext.GetAsNoTrackingQuery<Mission>()
            .Select(mission => new SelectItemDto
            {
                Id = mission.Id.ToString(),
                Text = mission.Title
            }).ToListAsync();
    }

    public async Task<bool> IsKodMeliDuplicate(long? id, string kodMeli)
    {
        return await _dbContext.GetAsNoTrackingQuery<Member>()
            .AnyAsync(member => member.Id != id && member.KodMeli == kodMeli);
    }

    public async Task<bool> IsMobileNumberDuplicate(long? id, string mobileNumber)
    {
        return await _dbContext.GetAsNoTrackingQuery<Member>()
            .AnyAsync(member => member.Id != id && member.MobileNumber == mobileNumber);
    }
}
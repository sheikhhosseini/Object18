﻿using AutoMapper;
using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
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
        AdvanceDataTable<MemberDataTableDto> data
    )
    {
        return await _dataTable.GetDataTable(_dbContext.GetEntitiesAsNoTrackingQuery<Member>(), data);
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
}
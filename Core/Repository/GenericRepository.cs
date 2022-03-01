using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity :BaseModel
{
    private readonly MainDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    public GenericRepository(MainDbContext context)
    {
        _context = context;
        _dbSet = this._context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetEntitiesQuery()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<TEntity> GetEntityById(long entityId)
    {
        return await _dbSet.SingleOrDefaultAsync(e => e.Id == entityId);
    }

    public async Task<TEntity> GetEntityByIdAsNoTracking(long entityId)
    {
        return await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == entityId);
    }

    public async Task AddEntity(TEntity entity)
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await _dbSet.AddAsync(entity);
    }

    public void UpdateEntity(TEntity entity)
    {
        entity.LastUpdateDate = DateTime.Now;
        _dbSet.Update(entity);
    }

    public void SoftRemoveEntity(TEntity entity)
    {
        entity.IsDelete = true;
        UpdateEntity(entity);
    }
    public async Task SoftRemoveEntityById(long entityId)
    {
        var entity = await GetEntityById(entityId);
        SoftRemoveEntity(entity);
    }

    public void HardRemoveEntity(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task HardRemoveEntityById(long entityId)
    {
        var entity = await GetEntityById(entityId);
        HardRemoveEntity(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
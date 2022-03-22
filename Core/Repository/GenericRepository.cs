using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository;


public class x
{
    public int Id { get; set; }
}

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

public static class DbContextExtension
{
    public static IQueryable<TEntity> GetEntitiesQuery<TEntity>(this MainDbContext dbContext) where TEntity : BaseModel
    {
        return dbContext.Set<TEntity>().AsQueryable();
    }

    public static async Task<TEntity> GetEntityById<TEntity>(this MainDbContext dbContext , long entityId) where TEntity : BaseModel
    {
        return await dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == entityId);
    }

    public static async Task AddEntity<TEntity>(this MainDbContext dbContext, TEntity entity) where TEntity : BaseModel
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await dbContext.Set<TEntity>().AddAsync(entity);
    }

    public static void UpdateEntity<TEntity>(this MainDbContext dbContext, TEntity entity) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
        dbContext.Set<TEntity>().Update(entity);
    }

    public static void SoftRemoveEntity<TEntity>(this MainDbContext dbContext, TEntity entity) where TEntity : BaseModel
    {
        entity.IsDelete = true;
        dbContext.Set<TEntity>().Update(entity);
    }

    public static async Task SoftRemoveEntityById<TEntity>(this MainDbContext dbContext, long entityId) where TEntity : BaseModel
    {
        var dbSet = dbContext.Set<TEntity>();
        var entity = await dbSet.SingleAsync(e => e.Id == entityId);
        entity.IsDelete = true;
        dbSet.Update(entity);
    }
}
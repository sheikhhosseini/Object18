using System.Reflection;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    public IQueryable<TEntity> GetEntitiesQuery<TEntity>() where TEntity : BaseModel
    {
        return Set<TEntity>().AsQueryable();
    }
    public IQueryable<TEntity> GetEntitiesAsNoTrackingQuery<TEntity>() where TEntity : BaseModel
    {
        return Set<TEntity>().AsNoTracking().AsQueryable();
    }
    public IQueryable<TEntity> GetEntityQueryById<TEntity>(long entityId) where TEntity : BaseModel
    {
        return Set<TEntity>().Where(e => e.Id == entityId);
    }

    public async Task AddEntityAsync<TEntity>(TEntity entity, bool log = false) where TEntity : BaseModel
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await AddAsync(entity);
        if (log)
        {
            await AddAsync(new LogCenter
            {
                EntityName = entity.GetType().Name,
                Action = LogType.Create,
                CreateDate = entity.CreateDate,
                EntityOldValues = null,
                EntityNewValues = JsonConvert.SerializeObject(entity)
            });
        }
    }

    public void UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
        //if (log)
        //{
        //    string oldValues = JsonConvert.SerializeObject(GetEntityQueryByIdAsNoTracking<TEntity>(entity.Id).FirstOrDefault());
        //    string newValues = JsonConvert.SerializeObject(entity);
        //    await AddAsync(new LogCenter
        //    {
        //        EntityName = entity.GetType().Name,
        //        Action = entity.IsDelete ? LogType.SoftDelete : LogType.Update,
        //        CreateDate = entity.LastUpdateDate,
        //        EntityOldValues = oldValues,
        //        EntityNewValues = newValues
        //    });
        //}
    }

    public async Task UpdateEntityAsNoTrackingAsync<TEntity>(TEntity entity, bool log = false) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
        Update(entity);
        //if (log)
        //{
        //    string oldValues = JsonConvert.SerializeObject(GetEntityQueryByIdAsNoTracking<TEntity>(entity.Id).FirstOrDefault());
        //    string newValues = JsonConvert.SerializeObject(entity);
        //    await AddAsync(new LogCenter
        //    {
        //        EntityName = entity.GetType().Name,
        //        Action = entity.IsDelete ? LogType.SoftDelete : LogType.Update,
        //        CreateDate = entity.LastUpdateDate,
        //        EntityOldValues = oldValues,
        //        EntityNewValues = newValues
        //    });
        //}
    }

    public void SoftRemoveEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.IsDelete = true;
        UpdateEntityAsync(entity);
    }

    public async Task SoftRemoveEntityByIdWithLogAsync<TEntity>(long entityId, bool log) where TEntity : BaseModel
    {
        var entity = await GetEntityQueryById<TEntity>(entityId).SingleAsync();
        SoftRemoveEntity(entity);
    }

    public async Task HardRemoveEntityWithLogAsync<TEntity>(TEntity entity, bool log) where TEntity : BaseModel
    {
        Remove(entity);
        if (log)
        {
            await AddAsync(new LogCenter
            {
                EntityName = entity.GetType().Name,
                Action = LogType.Delete,
                CreateDate = DateTime.Now,
                EntityOldValues = JsonConvert.SerializeObject(entity),
                EntityNewValues = null
            });
        }
    }

    public async Task HardRemoveEntityByIdWithLogAsync<TEntity>(long entityId, bool log) where TEntity : BaseModel
    {
        var entity = await GetEntityQueryById<TEntity>(entityId).SingleAsync();
        await HardRemoveEntityWithLogAsync(entity, log);
    }

    #region Tables
    public DbSet<LogCenter> LogCenters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    #endregion

    #region OnModelCreating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
using System.Linq.Expressions;
using System.Reflection;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    #region Functions

    public IQueryable<TEntity> GetAsNoTrackingQuery<TEntity>() where TEntity : BaseModel
    {
        return Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> GetQueryById<TEntity>(long entityId) where TEntity : BaseModel
    {
        return Set<TEntity>().Where(e => e.Id == entityId);
    }

    public IQueryable<TEntity> GetAsNoTrackingQueryById<TEntity>(long entityId) where TEntity : BaseModel
    {
        return Set<TEntity>().AsNoTracking().Where(e => e.Id == entityId);
    }

    public async Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await AddAsync(entity);
    }

    public void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
    }

    public void UpdateEntityAsNoTracking<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
        Update(entity);
    }

    public void SoftRemoveEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.IsDelete = true;
        UpdateEntity(entity);
    }

    public void HardRemoveEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        Remove(entity);
    }

    #endregion

    #region Tables
    public DbSet<LogCenter> LogCenters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Member> Members { get; set; }
    #endregion

    #region OnModelCreating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply SoftDelete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, "IsDelete"),
                    Expression.Constant(false));

                var lambda = Expression.Lambda(body, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}

//public IQueryable<TEntity> GetEntitiesQuery<TEntity>() where TEntity : BaseModel
//{
//    return Set<TEntity>().AsQueryable();
//}

//public async Task AddEntityAsync<TEntity>(TEntity entity, bool log = false) where TEntity : BaseModel
//{
//    entity.CreateDate = DateTime.Now;
//    entity.LastUpdateDate = entity.CreateDate;
//    await AddAsync(entity);
//    if (log)
//    {
//        await AddAsync(new LogCenter
//        {
//            EntityName = entity.GetType().Name,
//            Action = LogType.Create,
//            CreateDate = entity.CreateDate,
//            EntityOldValues = null,
//            EntityNewValues = JsonConvert.SerializeObject(entity)
//        });
//    }
//}

//public void UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseModel
//{
//    entity.LastUpdateDate = DateTime.Now;
//    if (log)
//    {
//        string oldValues = JsonConvert.SerializeObject(GetEntityQueryByIdAsNoTracking<TEntity>(entity.Id).FirstOrDefault());
//        string newValues = JsonConvert.SerializeObject(entity);
//        await AddAsync(new LogCenter
//        {
//            EntityName = entity.GetType().Name,
//            Action = entity.IsDelete ? LogType.SoftDelete : LogType.Update,
//            CreateDate = entity.LastUpdateDate,
//            EntityOldValues = oldValues,
//            EntityNewValues = newValues
//        });
//    }
//}
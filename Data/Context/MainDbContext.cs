using System.Reflection;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    public IQueryable<TEntity> GetEntityQueryById<TEntity>(long entityId) where TEntity : BaseModel
    {
        return Set<TEntity>().Where(e => e.Id == entityId);
    }

    public async Task AddEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await AddAsync(entity);
    }

    public void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.LastUpdateDate = DateTime.Now;
        Update(entity);
    }

    public void SoftRemoveEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        entity.IsDelete = true;
        UpdateEntity(entity);
    }

    public async Task SoftRemoveEntityById<TEntity>(long entityId) where TEntity : BaseModel
    {
        var entity = await GetEntityQueryById<TEntity>(entityId).SingleAsync();
        SoftRemoveEntity(entity);
    }

    public void HardRemoveEntity<TEntity>(TEntity entity) where TEntity : BaseModel
    {
        Remove(entity);
    }

    public async Task HardRemoveEntityById<TEntity>(long entityId) where TEntity : BaseModel
    {
        var entity = await GetEntityQueryById<TEntity>(entityId).SingleAsync();
        HardRemoveEntity(entity);
    }

    #region Tables

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    #endregion

    #region ManageCascade-OnModelCreateing

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
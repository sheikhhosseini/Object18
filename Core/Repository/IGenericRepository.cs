using Data.Models;

namespace Core.Repository;

public interface IGenericRepository<TEntity> : IDisposable where TEntity:BaseModel
{
    IQueryable<TEntity> GetEntitiesQuery();
    Task<TEntity> GetEntityById(long entityId);
    Task<TEntity> GetEntityByIdAsNoTracking(long entityId);
    Task AddEntity(TEntity entity);
    void UpdateEntity(TEntity entity);
    void SoftRemoveEntity(TEntity entity);
    Task SoftRemoveEntityById(long entityId);
    void HardRemoveEntity(TEntity entity);
    Task HardRemoveEntityById(long entityId);
    Task SaveChangesAsync();
}
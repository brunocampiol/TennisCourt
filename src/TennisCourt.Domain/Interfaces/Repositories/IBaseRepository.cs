
namespace TennisCourt.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {

        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);
        Task<TEntity> GetByIdAsync(Guid id);
        IQueryable<TEntity> GetAllQuery { get; }
        IQueryable<TEntity> GetAllQueryTracking { get; }
        bool Exists(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task UpdateAsync(TEntity entity);
    }
}

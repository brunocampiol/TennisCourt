using Microsoft.EntityFrameworkCore;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models.Base;
using TennisCourt.Infra.Data.Context;

namespace TennisCourt.Infra.Data.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> DbSet { get; }
        protected TennisCourtContext Db { get; }

        public BaseRepository(TennisCourtContext context)
        {
            Db = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAllQuery
        {
            get
            {
                return DbSet.AsNoTracking();
            }
        }

        public virtual IQueryable<TEntity> GetAllQueryTracking
        {
            get
            {
                return DbSet.AsQueryable<TEntity>();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await Db.SaveChangesAsync();
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            Db.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await Db.SaveChangesAsync();
            return entities;
        }

        public bool Exists(Guid id)
        {
            return DbSet.Any(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public TEntity GetById(Guid id)
        {
            return DbSet.FirstOrDefault(x => x.Id == id);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

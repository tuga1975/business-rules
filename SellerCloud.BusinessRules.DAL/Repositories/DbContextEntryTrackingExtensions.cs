using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SellerCloud.BusinessRules.DAL.Mapper;
using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public static class DbContextEntryTrackingExtensions
    {
        private static DbEntityEntry<TEntity> GetEntry<TEntity>(IDbContext dbContext, TEntity entity) where TEntity : class, IEntity, new() => 
            dbContext.ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Id == entity.Id);

        public static TDomain MapEntity<TEntity, TDomain>(this TDomain target, TEntity source)
            where TDomain : class, IEntity, new()
            where TEntity : class, IEntity
        {
            return BusinessRuleMapper.Instance.Map(source, target);
        }

        public static TEntity AddOrUpdateEntity<TEntity>(this IDbContext dbContext, TEntity entity)
            where TEntity : class, IEntity, new()
        {
            var entry = GetEntry(dbContext, entity);
            if (entry != null)
            {
                return entry.UpdateEntity(entity);
            }
            else
            {
                return dbContext.AddEntity(entity);
            }
        }

        public static TEntity AddEntity<TEntity>(this IDbContext dbContext, TEntity entity)
            where TEntity : class, IEntity, new()
        {
            return dbContext.Set<TEntity>().Add(entity);
        }
        private static TEntity UpdateEntity<TEntity>(this DbEntityEntry<TEntity> entry, TEntity entity)
            where TEntity : class, IEntity, new()
        {
            entry.Entity.MapEntity(entity);
            entry.State = EntityState.Modified;
            return entry.Entity;
        }


        public static TEntity UpdateEntity<TEntity>(this IDbContext dbContext, TEntity entity)
            where TEntity : class, IEntity, new()
        {
            var entry = GetEntry(dbContext, entity);
            if (entry != null)
            {
                return entry.UpdateEntity(entity);
            }
            else
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                return entity;
            }
        }

        public static void DeleteEntity<TEntity>(this IDbContext dbContext, TEntity entity)
            where TEntity : class, IEntity, new()
        {
            var entry = GetEntry(dbContext, entity);
            if (entry != null)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                dbContext.Entry(entity).State = EntityState.Deleted;
            }
        }
    }

}

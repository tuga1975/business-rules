using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.DAL.Mapper;
using SellerCloud.BusinessRules.Rules;
using Z.EntityFramework.Plus;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public abstract class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain, TEntity>
        where TDomain : class, IEntity, new()
        where TEntity : class, IEntity, new()
    {
        protected IDbContext Context;

        protected GenericRepository(IDbContext context)
        {
            Context = context;
            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.AutoDetectChangesEnabled = false;
        }

        protected DbSet<TEntity> Set() => Context.Set<TEntity>();

        public virtual List<TDomain> List() => Set().AsNoTracking().ProjectToList<TEntity, TDomain>();
        public virtual Task<List<TDomain>> ListAsync() => Set().AsNoTracking().ProjectToListAsync<TEntity, TDomain>();

        protected IQueryable<TEntity> FilterQuery(Expression<Func<TEntity, bool>> predicate) => Set().AsNoTracking().Where(predicate);

        public virtual List<TDomain> Filter(Expression<Func<TEntity, bool>> predicate) => FilterQuery(predicate).ProjectToList<TEntity, TDomain>();
        public virtual Task<List<TDomain>> FilterAsync(Expression<Func<TEntity, bool>> predicate) => FilterQuery(predicate).ProjectToListAsync<TEntity, TDomain>();

        protected virtual IQueryable<TEntity> InquireQuery(int id) => Set().AsNoTracking().Where(e => e.Id == id);
        
        public virtual TDomain Inquire(int id) => InquireQuery(id).ProjectFirstOrDefault<TEntity, TDomain>();
        public virtual Task<TDomain> InquireAsync(int id) => InquireQuery(id).ProjectFirstOrDefaultAsync<TEntity, TDomain>();
        
        public virtual TEntity Add(TDomain domain)
        {
            var dtoEntity = BusinessRuleMapper.Instance.Map<TDomain, TEntity>(domain);
            return Context.AddEntity(dtoEntity);
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TDomain> domainEntities) =>
            domainEntities.Select(Add).ToList();

        public virtual void Update(TDomain domain)
        {
            var dtoEntity = BusinessRuleMapper.Instance.Map<TDomain, TEntity>(domain);
            Context.UpdateEntity(dtoEntity);
        }

        public virtual void Delete(TDomain domain)
        {
            var dtoEntity = BusinessRuleMapper.Instance.Map<TDomain, TEntity>(domain);
            Context.DeleteEntity(dtoEntity);
        }

        public virtual IQueryable<TEntity> Delete(Expression<Func<TEntity, bool>> predicate) =>
            Set().Where(predicate);

        private DbEntityEntry<TEntity> GetEntry(TEntity entity) =>
            Context.ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Id == entity.Id);
    }

}

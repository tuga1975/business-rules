using SellerCloud.BusinessRules.DAL.Mapper;
using SellerCloud.BusinessRules.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public abstract class BusinessRulesEngineService<TDomain, TEntity, TRepository> : IBusinessRulesEngineService<TDomain, TEntity, TRepository>
        where TDomain : class, Rules.IEntity, new()
        where TEntity : class, Rules.IEntity, new()
        where TRepository : IGenericRepository<TDomain, TEntity>
    {
        protected readonly IBusinessRulesEngineUnitOfWork UnitOfWork;

        protected TRepository Repository { get; private set; }

        protected BusinessRulesEngineService(IBusinessRulesEngineUnitOfWork unitOfWork, TRepository repository)
        {
            this.UnitOfWork = unitOfWork;
            this.Repository = repository;
        }

        protected virtual TDomain PrepareDomainEntity(TDomain domainEntity) => domainEntity;

        public virtual List<TDomain> GetAll()
        {
            return Repository.List();
        }

        public virtual Task<List<TDomain>> GetAllAsync()
        {
            return Repository.ListAsync();
        }

        public virtual TDomain Inquire(int id)
        {
            return Repository.Inquire(id);
        }

        public virtual Task<TDomain> InquireAsync(int id)
        {
            return Repository.InquireAsync(id);
        }

        private TEntity CreateEntity(TDomain domainEntity)
        {
            if (domainEntity == null)
            {
                throw new ArgumentNullException(nameof(domainEntity));
            }

            return this.Repository.Add(PrepareDomainEntity(domainEntity));
        }

        public virtual int Create(TDomain domainEntity)
        {
            var entity = CreateEntity(domainEntity);

            this.UnitOfWork.Commit();

            BusinessRuleMapper.Instance.Map(entity, domainEntity);

            return entity.Id;
        }

        public virtual async Task<int> CreateAsync(TDomain domainEntity)
        {
            var entity = CreateEntity(domainEntity);

            await this.UnitOfWork.CommitAsync();

            BusinessRuleMapper.Instance.Map(entity, domainEntity);

            return entity.Id;
        }

        private IEnumerable<TEntity> CreateEntities(IEnumerable<TDomain> domainEntities)
        {
            if (domainEntities == null)
            {
                throw new ArgumentNullException(nameof(domainEntities));
            }

            return this.Repository.AddRange(domainEntities.Select(PrepareDomainEntity));
        }

        private IEnumerable<int> SetIdDomainEntities(IEnumerable<TEntity> entities, IEnumerable<TDomain> domainEntities)
        {
            return entities.Select((e, i) =>
            {
                var domainEntity = domainEntities.ElementAt(i);
                BusinessRuleMapper.Instance.Map(e, domainEntity);
                return e.Id;
            }).ToList();
        }

        public virtual IEnumerable<int> CreateRange(IEnumerable<TDomain> domainEntities)
        {
            var entities = CreateEntities(domainEntities);

            this.UnitOfWork.Commit();

            return SetIdDomainEntities(entities, domainEntities);
        }

        public virtual async Task<IEnumerable<int>> CreateRangeAsync(IEnumerable<TDomain> domainEntities)
        {
            var entities = CreateEntities(domainEntities);

            await this.UnitOfWork.CommitAsync();

            return SetIdDomainEntities(entities, domainEntities);
        }

        protected virtual void UpdateEntity(TDomain domainEntity)
        {
            if (domainEntity == null)
            {
                throw new ArgumentNullException(nameof(domainEntity));
            }

            this.Repository.Update(PrepareDomainEntity(domainEntity));
        }

        public virtual int Update(TDomain domainEntity)
        {
            UpdateEntity(domainEntity);
            return this.UnitOfWork.Commit();
        }

        public virtual Task<int> UpdateAsync(TDomain domainEntity)
        {
            UpdateEntity(domainEntity);
            return this.UnitOfWork.CommitAsync();
        }

        public virtual int Delete(int id)
        {
            var entity = Inquire(id);
            return Delete(entity);
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = Inquire(id);
            return await DeleteAsync(entity);
        }

        protected void DeleteEntity(TDomain domainEntity)
        {
            if (domainEntity == null)
            {
                throw new ArgumentNullException(nameof(domainEntity));
            }

            this.Repository.Delete(domainEntity);
        }

        protected void DeleteEntities(IEnumerable<TDomain> domainEntities)
        {
            if (domainEntities == null)
            {
                throw new ArgumentNullException(nameof(domainEntities));
            }

            foreach (var domainEntity in domainEntities)
            {
                this.Repository.Delete(domainEntity);
            }            
        }

        public virtual int Delete(TDomain domainEntity)
        {
            DeleteEntity(domainEntity);
            return this.UnitOfWork.Commit();
        }

        public virtual Task<int> DeleteAsync(TDomain domainEntity)
        {
            DeleteEntity(domainEntity);
            return this.UnitOfWork.CommitAsync();
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate) =>
            this.Repository.Delete(predicate).Delete();

        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate) =>
            this.Repository.Delete(predicate).DeleteAsync();

        public virtual int DeleteRange(IEnumerable<TDomain> domainEntities)
        {
            DeleteEntities(domainEntities);
            return this.UnitOfWork.Commit();
        }

        public virtual Task<int> DeleteRangeAsync(IEnumerable<TDomain> domainEntities)
        {
            DeleteEntities(domainEntities);
            return this.UnitOfWork.CommitAsync();
        }
    }
}

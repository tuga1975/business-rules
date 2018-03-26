using SellerCloud.BusinessRules.DAL.Repositories;
using SellerCloud.BusinessRules.Rules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public interface IBusinessRulesEngineService<TDomain, TEntity, TRepository> : IBusinessRuleService
        where TDomain : class, IEntity, new()
        where TEntity : class, IEntity, new()
        where TRepository : IGenericRepository<TDomain, TEntity>
    {
        List<TDomain> GetAll();
        Task<List<TDomain>> GetAllAsync();
        TDomain Inquire(int id);
        Task<TDomain> InquireAsync(int id);
        int Create(TDomain domainEntity);
        Task<int> CreateAsync(TDomain domainEntity);
        IEnumerable<int> CreateRange(IEnumerable<TDomain> domainEntities);
        Task<IEnumerable<int>> CreateRangeAsync(IEnumerable<TDomain> domainEntities);
        int Update(TDomain domainEntity);
        Task<int> UpdateAsync(TDomain domainEntity);
        int Delete(int id);
        Task<int> DeleteAsync(int id);
        int Delete(TDomain domainEntity);
        Task<int> DeleteAsync(TDomain domainEntity);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        int DeleteRange(IEnumerable<TDomain> domainEntities);
        Task<int> DeleteRangeAsync(IEnumerable<TDomain> domainEntities);
    }
}

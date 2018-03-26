using SellerCloud.BusinessRules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public interface IGenericRepository<TDomain, TEntity>
        where TDomain : class, IEntity, new()
        where TEntity : class, IEntity, new()
    {
        List<TDomain> List();
        Task<List<TDomain>> ListAsync();
        List<TDomain> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<List<TDomain>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
        TDomain Inquire(int id);
        Task<TDomain> InquireAsync(int id);
        TEntity Add(TDomain domain);
        IEnumerable<TEntity> AddRange(IEnumerable<TDomain> domainEntities);
        void Update(TDomain domain);
        void Delete(TDomain domain);
        IQueryable<TEntity> Delete(Expression<Func<TEntity, bool>> predicate);
    }

}

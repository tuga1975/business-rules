using SellerCloud.BusinessRules.Rules;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Mapper
{
    public static class BusinessRuleDomainEntityMapper
    {
        public static TDomain ProjectFirstOrDefault<TEntity, TDomain>(this IQueryable<TEntity> entity)
            where TDomain : class, IEntity, new()
            where TEntity : class, IEntity
        {
            var result = entity.FirstOrDefault();
            return BusinessRuleMapper.Instance.Map<TEntity, TDomain>(result);
        }

        public static async Task<TDomain> ProjectFirstOrDefaultAsync<TEntity, TDomain>(this IQueryable<TEntity> entity)
            where TDomain : class, IEntity, new()
            where TEntity : class, IEntity
        {
            var result = await entity.FirstOrDefaultAsync().ConfigureAwait(false);
            return BusinessRuleMapper.Instance.Map<TEntity, TDomain>(result);
        }

        public static List<TDomain> ProjectToList<TEntity, TDomain>(this IQueryable<TEntity> entity)
            where TDomain : class, IEntity, new()
            where TEntity : class, IEntity
        {
            var result = entity.ToList();
            return result.Select(e => BusinessRuleMapper.Instance.Map<TEntity, TDomain>(e)).ToList();
        }

        public static async Task<List<TDomain>> ProjectToListAsync<TEntity, TDomain>(this IQueryable<TEntity> entity)
            where TDomain : class, IEntity, new()
            where TEntity : class, IEntity
        {
            var result = await entity.ToListAsync().ConfigureAwait(false);
            return result.Select(e => BusinessRuleMapper.Instance.Map<TEntity, TDomain>(e)).ToList();
        }
    }
}

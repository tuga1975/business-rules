using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL
{
    public static class EntityOrderHelper
    {
        private static Action<TEntity> PrepareAction<TEntity, TKey, TCollectionKey>(Expression<Func<TEntity, IEnumerable<TKey>>> keySelector, Expression<Func<TKey, TCollectionKey>> innerKeySelector)
            where TEntity : class
            where TKey : class
        {
            var orderByMethod = typeof(Enumerable).GetMethods().Where(m => m.Name == nameof(Enumerable.OrderBy)).FirstOrDefault(m => m.GetParameters().Count() == 2);
            var orderByMethodGeneric = orderByMethod.MakeGenericMethod(typeof(TKey), typeof(TCollectionKey));

            var toListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList));
            var toListMethodGeneric = toListMethod.MakeGenericMethod(typeof(TKey));

            var orderMethodCallExpression = Expression.Call(orderByMethodGeneric, keySelector.Body, innerKeySelector);
            var toListMethodCallExpression = Expression.Call(toListMethodGeneric, orderMethodCallExpression);
            var assignExpression = Expression.Assign(keySelector.Body, toListMethodCallExpression);
            var lambdaExpression = Expression.Lambda<Action<TEntity>>(assignExpression, keySelector.Parameters);
            var action = lambdaExpression.Compile();
            return action;
        }

        public static TEntity InnerCollectionOrderBy<TEntity, TKey, TCollectionKey>(this TEntity entity, Expression<Func<TEntity, IEnumerable<TKey>>> keySelector, Expression<Func<TKey, TCollectionKey>> innerKeySelector)
            where TEntity : class
            where TKey : class
        {
            var action = PrepareAction(keySelector, innerKeySelector);
            action(entity);
            return entity;
        }

        public static async Task<TEntity> InnerCollectionOrderBy<TEntity, TKey, TCollectionKey>(this Task<TEntity> task, Expression<Func<TEntity, IEnumerable<TKey>>> keySelector, Expression<Func<TKey, TCollectionKey>> innerKeySelector)
            where TEntity : class
            where TKey : class
        {
            var entity = await task.ConfigureAwait(false);
            return entity.InnerCollectionOrderBy(keySelector, innerKeySelector);            
        }

        public static IEnumerable<TEntity> InnerCollectionOrderBy<TEntity, TKey, TCollectionKey>(this IEnumerable<TEntity> entities, Expression<Func<TEntity, IEnumerable<TKey>>> keySelector, Expression<Func<TKey, TCollectionKey>> innerKeySelector)
            where TEntity : class
            where TKey : class
        {
            return entities.Select(entity => entity.InnerCollectionOrderBy(keySelector, innerKeySelector));
        }

        public static async Task<List<TEntity>> InnerCollectionOrderBy<TEntity, TKey, TCollectionKey>(this Task<List<TEntity>> task, Expression<Func<TEntity, IEnumerable<TKey>>> keySelector, Expression<Func<TKey, TCollectionKey>> innerKeySelector)
            where TEntity : class
            where TKey : class
        {
            var entities = await task.ConfigureAwait(false);
            return entities.Select(entity => entity.InnerCollectionOrderBy(keySelector, innerKeySelector)).ToList();
        }
    }
}

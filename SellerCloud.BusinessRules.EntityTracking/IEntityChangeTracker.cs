using SellerCloud.BusinessRules.Rules;
using System;
using System.Linq.Expressions;

namespace SellerCloud.BusinessRules.EntityTracking
{
    public interface IEntityChangeTracker
    {
        IEntityChangeInformation TrackChanges(IRule rule, LambdaExpression lambda);
    }
}
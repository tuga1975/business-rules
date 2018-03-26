using SellerCloud.BusinessRules.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SellerCloud.BusinessRules.Extensions.ExtensionMethods
{
    public static partial class CollectionMethods
    {
        [ExpressionInfo(ExpressionType.Assign)]
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Func<T, T> func) => source.Select(i => func(i));

        [ExpressionInfo(ExpressionPurposeType.Filtering)]
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate, [ExpressionInfo(ExpressionType.Assign)] Func<T, T> func) =>
            source.Select(i => predicate(i) ? func(i) : i);

        //[BusinessRuleBooleanMethod]
        [BusinessRuleApplicabilityScope(ignore: true)]
        public static bool Contains<T>(this IEnumerable<T> source, T target) => source != null && Enumerable.Contains(source, target);
        [BusinessRuleBooleanMethod]
        public static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source != null && source.Any(predicate);

        [BusinessRuleBooleanMethod]
        [BusinessRuleApplicabilityScope(name: "Count Items")]
        public static int Count<T>(this IEnumerable<T> source) => Enumerable.Count(source);
        [BusinessRuleBooleanMethod]
        public static int Count<T>(this IEnumerable<T> source, Func<T, bool> predicate) => Enumerable.Count(source, predicate);

        #region Sum

        [BusinessRuleBooleanMethod]
        public static int Sum(this IEnumerable<int> source) => Enumerable.Sum(source);
        [BusinessRuleBooleanMethod]
        public static int Sum<T>(this IEnumerable<T> source, Func<T, int> predicate) => Enumerable.Sum(source, predicate);
        [BusinessRuleBooleanMethod]
        public static long Sum(this IEnumerable<long> source) => Enumerable.Sum(source);
        [BusinessRuleBooleanMethod]
        public static long Sum<T>(this IEnumerable<T> source, Func<T, long> predicate) => Enumerable.Sum(source, predicate);
        [BusinessRuleBooleanMethod]
        public static double Sum(this IEnumerable<double> source) => Enumerable.Sum(source);
        [BusinessRuleBooleanMethod]
        public static double Sum<T>(this IEnumerable<T> source, Func<T, double> predicate) => Enumerable.Sum(source, predicate);
        [BusinessRuleBooleanMethod]
        public static decimal Sum(this IEnumerable<decimal> source) => Enumerable.Sum(source);
        [BusinessRuleBooleanMethod]
        public static decimal Sum<T>(this IEnumerable<T> source, Func<T, decimal> predicate) => Enumerable.Sum(source, predicate);

        #endregion

        #region Average

        [BusinessRuleBooleanMethod]
        public static double Average(this IEnumerable<int> source) => Enumerable.Average(source);
        [BusinessRuleBooleanMethod]
        public static double Average<T>(this IEnumerable<T> source, Func<T, int> predicate) => Enumerable.Average(source, predicate);
        [BusinessRuleBooleanMethod]
        public static double Average(this IEnumerable<long> source) => Enumerable.Average(source);
        [BusinessRuleBooleanMethod]
        public static double Average<T>(this IEnumerable<T> source, Func<T, long> predicate) => Enumerable.Average(source, predicate);
        [BusinessRuleBooleanMethod]
        public static double Average(this IEnumerable<double> source) => Enumerable.Average(source);
        [BusinessRuleBooleanMethod]
        public static double Average<T>(this IEnumerable<T> source, Func<T, double> predicate) => Enumerable.Average(source, predicate);
        [BusinessRuleBooleanMethod]
        public static decimal Average(this IEnumerable<decimal> source) => Enumerable.Average(source);
        [BusinessRuleBooleanMethod]
        public static decimal Average<T>(this IEnumerable<T> source, Func<T, decimal> predicate) => Enumerable.Average(source, predicate);

        #endregion

        #region Min

        [BusinessRuleBooleanMethod]
        public static int Min(this IEnumerable<int> source) => Enumerable.Min(source);
        [BusinessRuleBooleanMethod]
        public static int Min<T>(this IEnumerable<T> source, Func<T, int> predicate) => Enumerable.Min(source, predicate);
        [BusinessRuleBooleanMethod]
        public static long Min(this IEnumerable<long> source) => Enumerable.Min(source);
        [BusinessRuleBooleanMethod]
        public static long Min<T>(this IEnumerable<T> source, Func<T, long> predicate) => Enumerable.Min(source, predicate);
        [BusinessRuleBooleanMethod]
        public static double Min(this IEnumerable<double> source) => Enumerable.Min(source);
        [BusinessRuleBooleanMethod]
        public static double Min<T>(this IEnumerable<T> source, Func<T, double> predicate) => Enumerable.Min(source, predicate);
        [BusinessRuleBooleanMethod]
        public static decimal Min(this IEnumerable<decimal> source) => Enumerable.Min(source);
        [BusinessRuleBooleanMethod]
        public static decimal Min<T>(this IEnumerable<T> source, Func<T, decimal> predicate) => Enumerable.Min(source, predicate);

        #endregion

        #region Max

        [BusinessRuleBooleanMethod]
        public static int Max(this IEnumerable<int> source) => Enumerable.Max(source);
        [BusinessRuleBooleanMethod]
        public static int Max<T>(this IEnumerable<T> source, Func<T, int> predicate) => Enumerable.Max(source, predicate);
        [BusinessRuleBooleanMethod]
        public static long Max(this IEnumerable<long> source) => Enumerable.Max(source);
        [BusinessRuleBooleanMethod]
        public static long Max<T>(this IEnumerable<T> source, Func<T, long> predicate) => Enumerable.Max(source, predicate);
        [BusinessRuleBooleanMethod]
        public static double Max(this IEnumerable<double> source) => Enumerable.Max(source);
        [BusinessRuleBooleanMethod]
        public static double Max<T>(this IEnumerable<T> source, Func<T, double> predicate) => Enumerable.Max(source, predicate);
        [BusinessRuleBooleanMethod]
        public static decimal Max(this IEnumerable<decimal> source) => Enumerable.Max(source);
        [BusinessRuleBooleanMethod]
        public static decimal Max<T>(this IEnumerable<T> source, Func<T, decimal> predicate) => Enumerable.Max(source, predicate);

        #endregion
    }
}

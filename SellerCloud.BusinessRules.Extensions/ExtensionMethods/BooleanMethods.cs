using SellerCloud.BusinessRules.Extensions.Helpers;
using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Extensions.ExtensionMethods
{
    public static class BooleanMethods
    {
        private static bool HandleNullable<T>(T source, Func<T, bool> action)
        {
            if (typeof(T).IsNullableType() && source == null)
            {
                return false;
            }

            return action(source);
        }
        public static bool Equals<T>(this T source, T argument) => HandleNullable(source, s => s.Equals(argument));
        public static bool NotEquals<T>(this T source, T argument) => HandleNullable(source, s => !s.Equals(argument));

        public static bool IsInList<T>(this T source, IEnumerable<T> arguments) => HandleNullable(source, s => arguments.Contains(s));
        public static bool IsNotInList<T>(this T source, IEnumerable<T> arguments) => HandleNullable(source, s => !arguments.Contains(s));

        public static bool Contains(this string source, string target) => (source == null || target == null) ? source == target : source.Contains(target);
        public static bool NotContains(this string source, string target) => (source == null || target == null) ? source != target : !source.Contains(target);

        public static bool StartsWith(this string source, string target) => (source == null || target == null) ? source == target : source.StartsWith(target);
        public static bool EndsWith(this string source, string target) => (source == null || target == null) ? source == target : source.EndsWith(target);

        public static bool GreaterThan<T>(this T source, T target) where T : IComparable => HandleNullable(source, s => s.CompareTo(target) > 0);
        public static bool GreaterThanOrEqual<T>(this T source, T target) where T : IComparable => HandleNullable(source, s => s.CompareTo(target) >= 0);
        public static bool LessThan<T>(this T source, T target) where T : IComparable => HandleNullable(source, s => s.CompareTo(target) < 0);
        public static bool LessThanOrEqual<T>(this T source, T target) where T : IComparable => HandleNullable(source, s => s.CompareTo(target) <= 0);
        public static bool Between<T>(this T source, T left, T right) where T : IComparable => HandleNullable(source, s => s.GreaterThan(left) && s.LessThan(right));
        public static bool BetweenOrEqual<T>(this T source, T left, T right) where T : IComparable => HandleNullable(source, s => s.GreaterThanOrEqual(left) && s.LessThanOrEqual(right));
    }
}

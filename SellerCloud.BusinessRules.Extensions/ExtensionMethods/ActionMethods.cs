using System;

namespace SellerCloud.BusinessRules.Extensions.ExtensionMethods
{
    public static class ActionMethods
    {
        public static T Assign<T>(this T source, T newValue) => source = newValue;

        public static string Concat(this string source, string valueToConcatenate) => source + valueToConcatenate;

        public static string Trim(this string source) => source.Trim();
        public static string TrimStart(this string source) => source.TrimStart();
        public static string TrimEnd(this string source) => source.TrimEnd();

        public static double Round(this double source, int decimals) => Math.Round(source, decimals);
        public static decimal Round(this decimal source, int decimals) => Math.Round(source, decimals);

        public static double Ceiling(this double source) => Math.Ceiling(source);
        public static decimal Ceiling(this decimal source) => Math.Ceiling(source);

        public static double Floor(this double source) => Math.Floor(source);
        public static decimal Floor(this decimal source) => Math.Floor(source);

        public static int Add(this int source, int valueToAdd) => source + valueToAdd;
        public static long Add(this long source, long valueToAdd) => source + valueToAdd;
        public static double Add(this double source, double valueToAdd) => source + valueToAdd;
        public static decimal Add(this decimal source, decimal valueToAdd) => source + valueToAdd;

        public static int Subtract(this int source, int valueToAdd) => source - valueToAdd;
        public static long Subtract(this long source, long valueToAdd) => source - valueToAdd;
        public static double Subtract(this double source, double valueToAdd) => source - valueToAdd;
        public static decimal Subtract(this decimal source, decimal valueToAdd) => source - valueToAdd;

        public static int Multiply(this int source, int valueToAdd) => source * valueToAdd;
        public static long Multiply(this long source, long valueToAdd) => source * valueToAdd;
        public static double Multiply(this double source, double valueToAdd) => source * valueToAdd;
        public static decimal Multiply(this decimal source, decimal valueToAdd) => source * valueToAdd;

        public static int Divide(this int source, int valueToAdd) => source / valueToAdd;
        public static long Divide(this long source, long valueToAdd) => source / valueToAdd;
        public static double Divide(this double source, double valueToAdd) => source / valueToAdd;
        public static decimal Divide(this decimal source, decimal valueToAdd) => source / valueToAdd;

    }
}

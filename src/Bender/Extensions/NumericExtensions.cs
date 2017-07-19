using System;
namespace Bender.Extensions
{
    public static class NumericExtensions
    {
        public static bool IsNonNumeric(this float value)
        {
            return float.IsNaN(value) || float.IsInfinity(value) || 
                float.IsNegativeInfinity(value) || float.IsPositiveInfinity(value);
        }

        public static bool IsNonNumeric(this double value)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ||
                   double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value);
        }
    }
}

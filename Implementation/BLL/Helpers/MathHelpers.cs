using System;

namespace Implementation.BLL.Helpers
{
    public static class MathHelpers
    {

        public static double PreservePrecision(double number)
        {
            return Math.Round(number * 1000000000.0) / 1000000000.0;
        }

        public static double CurrencyPrecision(double number)
        {
            return Math.Round(number * 100.0) / 100.0;
        }

        public static double GreedyCurrencyPrecision(double number)
        {
            return Math.Floor(number * 100.0) / 100.0;
        }

    }
}

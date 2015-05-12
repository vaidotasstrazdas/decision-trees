using System;

namespace Implementation.BLL.Helpers
{
    public static class MathHelpers
    {

        public static double PreservePrecision(double number)
        {
            return Math.Round(number * 1000000000.0) / 1000000000.0;
        }

    }
}

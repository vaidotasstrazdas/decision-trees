using System.Linq;

namespace StatisticsComparer
{
    public static class InfoHelper
    {

        public static string GetPeriod(string filePath)
        {
            var periodExtension = filePath.Split('_').Last();
            var parts = periodExtension.Split('.');
            var period = parts[0];

            return period;
        }

        public static string GetMonth(string filePath)
        {
            var parts = filePath.Split('_');
            var month = parts[2];

            return month;

        }

    }
}

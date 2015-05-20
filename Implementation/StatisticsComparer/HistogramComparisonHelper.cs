using System;
using System.Collections.Generic;
using System.IO;
using Bridge.IBLL.Interfaces;

namespace StatisticsComparer
{
    public static class HistogramComparisonHelper
    {

        public static IHistogramService Service { get; set; }
        public static string HistogramPath { get; set; }

        public static void PreparePeriod(List<string> files, string period)
        {
            var path = Path.Combine(HistogramPath, string.Format("Histogram_P{0}.csv", period));
            Console.WriteLine("Preparing period {0} histogram.", period);
            foreach (var filePath in files)
            {
                var periodOfFile = InfoHelper.GetPeriod(filePath);
                if (periodOfFile != period)
                {
                    continue;
                }
                Service.ReadStatisticsData(filePath);
                Service.PrepareData();
            }

            var statistics = Service.CalculateStatistics();
            Service.AddToRepository(statistics);
            Service.CommitToRepository(path);
            Console.WriteLine("Prepared.");
            Service.Clear();

        }

        public static void PrepareMonth(List<string> files, string month)
        {
            var path = Path.Combine(HistogramPath, string.Format("Histogram_M{0}.csv", month));
            Console.WriteLine("Preparing period {0} histogram.", month);
            foreach (var filePath in files)
            {
                var monthOfFile = InfoHelper.GetMonth(filePath);
                if (monthOfFile != month)
                {
                    continue;
                }
                Service.ReadStatisticsData(filePath);
                Service.PrepareData();
            }

            var statistics = Service.CalculateStatistics();
            Service.AddToRepository(statistics);
            Service.CommitToRepository(path);
            Console.WriteLine("Prepared.");
            Service.Clear();

        }

        public static void PreparePeriodForMonth(List<string> files, string period, string month)
        {
            var path = Path.Combine(HistogramPath, string.Format("Histogram_P{0}M{1}.csv", period, month));
            Console.WriteLine("Preparing month {0} histogram for period {1}.", month, period);
            foreach (var filePath in files)
            {
                var monthOfFile = InfoHelper.GetMonth(filePath);
                var periodOfFile = InfoHelper.GetPeriod(filePath);
                if (monthOfFile != month || periodOfFile != period)
                {
                    continue;
                }
                Service.ReadStatisticsData(filePath);
                Service.PrepareData();
            }

            var statistics = Service.CalculateStatistics();
            Service.AddToRepository(statistics);
            Service.CommitToRepository(path);
            Console.WriteLine("Prepared.");
            Service.Clear();

        }

    }
}

﻿using System;
using System.Collections.Generic;
using Bridge.IBLL.Interfaces;

namespace StatisticsComparer
{
    public static class StatisticsComparisonHelper
    {

        public static IStatisticsService Service { get; set; }

        public static void PrepareAllTime(List<string> files)
        {
            Service.BluePrint = "AllTime";

            Console.WriteLine("Preparing all time statistics.");
            foreach (var filePath in files)
            {
                Service.ReadStatisticsData(filePath);
                Service.PrepareData();
            }

            var statistics = Service.CalculateStatistics();
            Service.AddToRepository(statistics);
            Console.WriteLine("Prepared.");
            Service.ResetSequence();

        }

        public static void PreparePeriod(List<string> files, string period)
        {
            Service.BluePrint = string.Format("Period{0}", period);
            Console.WriteLine("Preparing period {0} statistics.", period);
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
            Console.WriteLine("Prepared.");
            Service.ResetSequence();

        }

        public static void PrepareMonth(List<string> files, string month)
        {
            Service.BluePrint = string.Format("Month{0}", month);
            Console.WriteLine("Preparing period {0} statistics.", month);
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
            Console.WriteLine("Prepared.");
            Service.ResetSequence();

        }

        public static void PreparePeriodForMonth(List<string> files, string period, string month)
        {
            Service.BluePrint = string.Format("Month{0}Period{1}", month, period);
            Console.WriteLine("Preparing month {0} statistics for period {1}.", month, period);
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
            Console.WriteLine("Prepared.");
            Service.ResetSequence();

        }

    }
}

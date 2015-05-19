#region Usings
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Bridge.IBLL.Interfaces;
#endregion


namespace StatisticsComparer
{
    public class Application : IApplication
    {
        private readonly IStatisticsService _statisticsService;

        public Application(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            StatisticsComparisonHelper.Service = _statisticsService;
        }

        public void Run()
        {
            var inputPath = ConfigurationManager.AppSettings["StatisticsInputPath"];
            var outputPath = Path.Combine(ConfigurationManager.AppSettings["StatisticsOutputPath"], "Results.csv");
            var files = Directory.GetFiles(inputPath).Where(x => StatisticsComparisonHelper.GetPeriod(x) != "21600").ToList();
            var periods = new List<string> { "300", "600", "900", "1800" };
            var months = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            StatisticsComparisonHelper.PrepareAllTime(files);
            foreach (var period in periods)
            {
                StatisticsComparisonHelper.PreparePeriod(files, period);
            }

            foreach (var month in months)
            {
                StatisticsComparisonHelper.PrepareMonth(files, month);
            }

            foreach (var period in periods)
            {
                foreach (var month in months)
                {
                    StatisticsComparisonHelper.PreparePeriodForMonth(files, period, month);
                }
            }

            _statisticsService.CommitToRepository(outputPath);
        }

    }
}

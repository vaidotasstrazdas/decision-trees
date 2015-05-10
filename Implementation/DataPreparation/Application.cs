#region Usings
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces;
#endregion

namespace Implementation.DataPreparation
{
    public class Application : IApplication
    {
        private readonly IYahooService _yahooService;

        public Application(IYahooService yahooService)
        {
            _yahooService = yahooService;
        }

        public void Run()
        {
            Console.WriteLine("Reading CSV data.");
            _yahooService.ReadCsv(ConfigurationManager.AppSettings["YahooTestDataFile"]);
            Console.WriteLine("CSV read successfully.");
            Console.WriteLine("Preparing CSV data.");

            List<YahooNormalized> normalizedData = _yahooService.PrepareData().ToList();
            Console.WriteLine("Data prepared successfully.");

            Console.WriteLine("Saving normalized Yahoo data for decision tree generation software.");
            _yahooService.SaveYahooData(normalizedData, ConfigurationManager.AppSettings["YahooDataOutputPath"]);
            Console.WriteLine("Decision tree data for Yahoo saved successfully.");

            Console.WriteLine("Press Enter to Exit.");
            Console.ReadLine();
        }
    }
}

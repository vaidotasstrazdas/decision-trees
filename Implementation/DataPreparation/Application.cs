#region Usings
using System;
using System.Collections.Generic;
using System.Configuration;

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

            IEnumerable<YahooNormalized> normalizedData = _yahooService.PrepareData();
            Console.WriteLine("Date,Close,Volatility");
            foreach (var dataRecord in normalizedData)
            {
                Console.WriteLine("{0},{1},{2}", dataRecord.Date, dataRecord.Close, dataRecord.Volatility);
            }

            Console.WriteLine("Press Enter to Exit.");
            Console.ReadLine();
        }
    }
}

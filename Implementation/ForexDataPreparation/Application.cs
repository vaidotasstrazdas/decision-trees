#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Bridge.IBLL.Interfaces;

#endregion

namespace Implementation.ForexDataPreparation
{
    public class Application : IApplication
    {
        private readonly IForexService _forexService;

        public Application(IForexService forexService)
        {
            _forexService = forexService;
        }

        public void Run()
        {
            Console.Write("Enter currency pair: ");
            var currencyPair = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(currencyPair))
            {
                Console.WriteLine("Wrong currency pair.");
                return;
            }

            Console.Write("Enter year: ");
            var year = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(year))
            {
                Console.WriteLine("Wrong year.");
                return;
            }

            Console.Write("Enter months: ");
            var monthsRaw = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(monthsRaw))
            {
                Console.WriteLine("Wrong month.");
                return;
            }
            var months = monthsRaw.Split(',');

            foreach (var month in months)
            {
                Console.WriteLine("Configuration Set: {0},{1},{2}", currencyPair, year, month);

                var periods = new List<int> { 300, 600, 900, 1800, 21600 };
                var forexInputPath = Path.Combine(ConfigurationManager.AppSettings["ForexDataInputPath"], currencyPair, year, string.Format("EURUSD-2014-{0}.csv", month));
                var forexOutputPath = Path.Combine(ConfigurationManager.AppSettings["ForexDataOutputPath"], currencyPair, year, month);

                if (!Directory.Exists(forexOutputPath))
                {
                    Directory.CreateDirectory(forexOutputPath);
                }

                Console.WriteLine("Application started.");
                Console.WriteLine("Forex Input ForexTreesPath Now: {0}", forexInputPath);

                Console.WriteLine("Reading Forex CSV");

                _forexService.ReadCsv(forexInputPath);

                Console.WriteLine("CSV File Read successfully!");
                Console.WriteLine("Starting to prepare Forex trees.");

                foreach (var period in periods)
                {
                    Console.WriteLine("Chosen period: {0}s", period);
                    Console.WriteLine("Preparing data for selected period.");
                    var data = _forexService.PrepareData(period);
                    Console.WriteLine("Data for selected period prepared.");
                    Console.WriteLine("Saving data for selected period.");
                    _forexService.SaveForexData(data, period, forexOutputPath);
                    Console.WriteLine("Data saved successfully for selected period.");
                }

            }

            Console.WriteLine("Forex trees prepared successfully.");
            Console.WriteLine("Press Enter to Exit.");

            Console.ReadLine();
        }
    }
}

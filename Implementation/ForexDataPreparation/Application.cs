#region Usings

using System;
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
            var dateTime = DateTime.ParseExact("20140101 21:55:34.378", "yyyyMMdd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

            Console.WriteLine(dateTime);
            Console.ReadLine();
            var currencyPair = "EURUSD";
            var year = "2014";
            var month = "01";
            var forexPath = Path.Combine(ConfigurationManager.AppSettings["ForexDataInputPath"], currencyPair, year, string.Format("EURUSD-2014-{0}.csv", month));
            
            Console.WriteLine("Application started.");
            Console.WriteLine("Forex Path Now: {0}", forexPath);

            Console.WriteLine("Reading Forex CSV");

            _forexService.ReadCsv(forexPath);

            Console.WriteLine("CSV File Read successfully!");
            Console.WriteLine("Starting to prepare Forex data.");

            _forexService.PrepareData(3600);

            Console.ReadLine();
        }
    }
}

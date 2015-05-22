using System;
using System.Collections.Generic;
using ConsoleFramework;
using Shared.DecisionTrees.DataStructure;

namespace ForexTradeModel
{
    public class Application : IApplication
    {
        private readonly TradingModel _model;

        public Application(TradingModel model)
        {
            _model = model;
        }

        public void Run()
        {
            var currency = Reader.ReadString("Enter currency (default: EURUSD): ", "EURUSD");
            var year = Reader.ReadString("Enter year (default: 2014): ", "2014");
            var periods = Reader.ReadList("Enter periods for trading (default: 300,600,900,1800): ", new List<string> {"300", "600", "900", "1800"});
            var cases = Reader.ReadInt("Enter minimum number of cases in chunk (default: 500): ", 500);
            var statisticsPath = Reader.ReadString("Enter folder of statistics (default: D:\\Personal\\BachelorYahooForex\\Statistics): ", "D:\\Personal\\BachelorYahooForex\\Statistics");
            var forexTreesPath = Reader.ReadString("Enter folder of Forex Trees (default: D:\\Personal\\BachelorYahooForex\\ForexTrees): ", "D:\\Personal\\BachelorYahooForex\\ForexTrees");

            Console.WriteLine("Initializing the model.");
            _model.Initialize(currency, year, periods, cases, statisticsPath, forexTreesPath);
            Console.WriteLine("Initialization complete.");

            Console.WriteLine("Preparing for C5.0 algorithm.");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            Console.WriteLine("Prepared.");

            // Balance: 100000.0, Bid Size: 100.0, Margin Ratio: 0.02
            _model.SetModelQuantities(100000.0, 100.0);

            Console.WriteLine("Starting to trade.");
            _model.Trade();
            Console.WriteLine("Trading ended. Press Enter to exit.");

            Console.ReadLine();

        }
    }

}

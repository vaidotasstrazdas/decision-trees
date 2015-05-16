using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BatchMaker
{
    class Program
    {
        private static void Main(string[] args)
        {

            Console.Write("Enter path of ForexTrees folder: ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Wrong path entered.");
                Console.ReadLine();
                return;
            }

            var fullPath = Path.Combine(path, "ForexTrees");
            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine("Path {0} does not exist.", fullPath);
                Console.ReadLine();
                return;
            }

            StringBuilder commandBuilder = new StringBuilder();
            var builders = new List<StringBuilder>();
            var index = 0;
            var skipMonths = 1;
            var currencies = Directory.GetDirectories(fullPath);
            foreach (var currency in currencies)
            {
                var currencyPath = Path.Combine(fullPath, currency);
                var years = Directory.GetDirectories(currencyPath);
                foreach (var year in years)
                {
                    var yearsPath = Path.Combine(currencyPath, year);
                    var months = Directory.GetDirectories(yearsPath);
                    foreach (var month in months)
                    {
                        if (skipMonths-- > 0)
                        {
                            continue;
                        }
                        var periodsPath = Path.Combine(yearsPath, month);
                        var periods = Directory.GetDirectories(periodsPath);
                        foreach (var period in periods)
                        {
                            var intPeriod = int.Parse(period.Split('\\').Last());
                            var treesPath = Path.Combine(periodsPath, period);
                            var trees = Directory.GetFiles(treesPath, "Forex_*.data");

                            for (var i = 0; i < trees.Length; i++)
                            {
                                index++;
                                if (index % 1500 == 0)
                                {
                                    builders.Add(commandBuilder);
                                    commandBuilder = new StringBuilder();
                                }
                                if (intPeriod < 21600)
                                {
                                    commandBuilder.AppendLine(GetCommand(treesPath, "c45", i));
                                }
                                commandBuilder.AppendLine(GetCommand(treesPath, "c50", i));
                            }

                        }
                    }
                }
            }

            if (commandBuilder.Length > 0)
            {
                builders.Add(commandBuilder);
            }

            for (var i = 0; i < builders.Count; i++)
            {
                var builder = builders[i];
                var batchFile = Path.Combine(path, "Run", string.Format("RunTrees_{0}.bat", i));

                File.WriteAllBytes(batchFile, Encoding.UTF8.GetBytes(builder.ToString()));
                
            }

            Console.Write("Done. Press Enter to exit.");
            Console.ReadLine();

        }

        private static string GetCommand(string treesPath, string algorithm, int tree)
        {
            bool combineFlag = false;
            var commandPath = "";
            var folders = treesPath.Split('\\');
            foreach (var folder in folders)
            {
                if (folder == "ForexTrees")
                {
                    combineFlag = true;
                }

                if (!combineFlag)
                {
                    continue;
                }

                commandPath = Path.Combine(commandPath, folder);

            }

            commandPath = Path.Combine(commandPath, string.Format("Forex_{0}", tree));

            return string.Concat(algorithm, " -f ", commandPath, " -b > ", commandPath, ".", algorithm.ToUpper(), ".done");

        }
    }
}

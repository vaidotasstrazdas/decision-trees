using System;
using System.IO;
using System.Text;

namespace Implementation.StatisticsCollection
{
    class Program
    {
        static void Main()
        {

            Console.Write("Enter path of ForexTrees folder: ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "D:\\Personal\\BachelorYahooForex";
            }

            var fullPath = Path.Combine(path, "ForexTrees");
            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine("Path {0} does not exist.", fullPath);
                Console.ReadLine();
                return;
            }

            Console.Write("Enter output path for your statistics: ");
            var outputPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = "D:\\Personal\\BachelorYahooForex\\Statistics";
            }
            if (!Directory.Exists(outputPath))
            {
                Console.WriteLine("Path {0} does not exist.", outputPath);
                Console.ReadLine();
                return;
            }

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
                        var periodsPath = Path.Combine(yearsPath, month);
                        var periods = Directory.GetDirectories(periodsPath);
                        foreach (var period in periods)
                        {
                            var treesPath = Path.Combine(periodsPath, period);
                            var trees = Directory.GetFiles(treesPath, "Forex_*.data");
                            var fileName = string.Format("{0}_{1}_{2}_{3}.csv", Clean(currency), Clean(year), Clean(month), Clean(period));
                            var filePath = Path.Combine(outputPath, fileName);
                            var builder = new StringBuilder();
                            Console.WriteLine("Saving to {0}.", filePath);
                            builder.AppendLine("Algorithm,Cases,Errors,Chunk");
                            for (var i = 0; i < trees.Length; i++)
                            {
                                SaveData(builder, treesPath, Clean(period), i);
                            }
                            File.WriteAllText(filePath, builder.ToString());
                            Console.WriteLine("Saved.");
                        }
                    }
                }
            }

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();

        }

        private static string Clean(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            return fileName;
        }

        private static void SaveData(StringBuilder builder, string treesPath, string period, int chunkNumber)
        {
            
            var c45File = string.Format("Forex_{0}.C45.done", chunkNumber);
            var c50File = string.Format("Forex_{0}.C50.done", chunkNumber);
            var chunk45Path = Path.Combine(treesPath, c45File);
            var chunk50Path = Path.Combine(treesPath, c50File);

            TreeStatistics c45Statistics = null;
            if (period != "21600")

            {
                c45Statistics = StatisticsCollector.CollectC45(chunk45Path);
                if (c45Statistics == null)
                {
                    return;
                }
                c45Statistics.ChunkNumber = chunkNumber;
            }

            var c50Statistics = StatisticsCollector.CollectC50(chunk50Path);
            if (c50Statistics == null)
            {
                return;
            }
            c50Statistics.ChunkNumber = chunkNumber;

            if (c45Statistics != null)
            {
                builder.AppendLine(string.Format("{0},{1},{2},{3}", c45Statistics.Algorithm, c45Statistics.DataSetSize,
                        c45Statistics.Errors, c45Statistics.ChunkNumber));
            }
            builder.AppendLine(string.Format("{0},{1},{2},{3}", c50Statistics.Algorithm, c50Statistics.DataSetSize,
                c50Statistics.Errors, c50Statistics.ChunkNumber));

        }
    }
}

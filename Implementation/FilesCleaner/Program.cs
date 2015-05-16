using System;
using System.IO;

namespace FilesCleaner
{
    class Program
    {
        static void Main()
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

                            for (var i = 0; i < trees.Length; i++)
                            {
                                DeleteFile(treesPath, "tmp", i);
                                DeleteFile(treesPath, "tree", i);
                                DeleteFile(treesPath, "unpruned", i);
                            }

                        }
                    }
                }
            }

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();

        }

        private static void DeleteFile(string treesPath, string extension, int splitNumber)
        {
            var filePath = Path.Combine(treesPath, string.Format("Forex_{0}.{1}", splitNumber, extension));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}

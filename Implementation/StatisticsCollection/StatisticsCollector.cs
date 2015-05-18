using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Implementation.StatisticsCollection
{
    public static class StatisticsCollector
    {

        public static TreeStatistics CollectC45(string c45Path)
        {
            if (!File.Exists(c45Path))
            {
                return null;
            }

            string contents = File.ReadAllText(c45Path);

            var statistics = new TreeStatistics
            {
                Algorithm = "C4.5",
                DataSetSize = GetSize(contents),
                Errors = GetErrorsC45(contents)
            };

            if (statistics.DataSetSize == -1 || statistics.Errors == -1)
            {
                return null;
            }

            return statistics;
        }

        public static TreeStatistics CollectC50(string chunk50Path)
        {
            if (!File.Exists(chunk50Path))
            {
                return null;
            }

            string contents = File.ReadAllText(chunk50Path);

            var statistics = new TreeStatistics
            {
                Algorithm = "C5.0",
                DataSetSize = GetSize(contents),
                Errors = GetErrorsC50(contents)
            };

            if (statistics.DataSetSize == -1 || statistics.Errors == -1)
            {
                return null;
            }

            return statistics;
        }

        private static int GetErrorsC50(string contents)
        {
            var parts = contents.Split(new[] {"Evaluation on training data"}, StringSplitOptions.None);
            var match = Regex.Match(parts[1], @"0\s+\d+\s+(\d+)\(\s*\d+.\d+%\)");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            match = Regex.Match(parts[1], @"\d+\s+(\d+)\(\s*\d+\.\d+%\)\s+<<");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return -1;
        }

        private static int GetErrorsC45(string contents)
        {
            var match = Regex.Match(contents, @"(\d+)\(\s*\d+\.\d+%\)");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return -1;
        }

        private static int GetSize(string contents)
        {
            var match = Regex.Match(contents, @"Read (\d+) cases");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return -1;
        }

    }
}

#region Usings
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bridge.IDLL.Interfaces;
#endregion

namespace Implementation.DLL
{
    public class ForexMarketPathRepository : IForexMarketPathRepository
    {

        #region Constructors and Destructors
        public ForexMarketPathRepository()
        {
            Paths = new List<string>();
        }
        #endregion

        #region Implemented Interfaces

        #region IForexMarketPathRepository
        public string ForexTreesPath { get; set; }
        public List<string> Paths { get; private set; }

        public void SetPaths(int startingMonth, string period, int startingChunk)
        {
            AddPaths(ForexTreesPath, startingMonth, period, startingChunk);
            for (var month = startingMonth + 1; month <= 12; month++)
            {
                AddPaths(ForexTreesPath, month, period, 0);
            }
        }

        public int GetChunk(string filePath)
        {
            var parts = filePath.Split('_');
            var chunkString = parts[1].Replace(".data", string.Empty);

            return int.Parse(chunkString);
        }
        #endregion

        #endregion

        #region Methods
        private void AddPaths(string forexTreesPath, int month, string period, int startingChunk)
        {
            var dataFileDirectory = Path.Combine(forexTreesPath, string.Format("{0:00}", month), period);
            if (!Directory.Exists(dataFileDirectory))
            {
                return;
            }
            var paths = Directory.GetFiles(dataFileDirectory, "Forex_*.data")
                        .Where(x => GetChunk(x) >= startingChunk)
                        .OrderBy(GetChunk).ToList();
            Paths.AddRange(paths);
        }
        #endregion

    }
}
